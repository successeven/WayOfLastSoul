using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(HeroManager))]
[RequireComponent(typeof(HeroMotor))]
public class HeroController : MonoBehaviour
{
		bool _doubleBlock = false;
		float _lastBlockClickTime = 0;
		bool _holdBlock = false;
		float _catchTime = .3f;

		[NonSerialized]
		public bool _holdAttack = false;

		[NonSerialized]
		public bool _interfaceBlocked = true;


		bool jump = false;

		void FixedUpdate()
		{

				if (Hero.instance.Manager._Health <= 0)
						return;

				if (_interfaceBlocked)
						return;

				Hero.instance.Motor.Move(CrossPlatformInputManager.GetAxis("Horizontal"), jump);
				jump = false;
		}


		private void Update()
		{
				if (Hero.instance.Manager._Health <= 0)
						return;

				if (_interfaceBlocked)
						return;

				if (CrossPlatformInputManager.GetButtonDown("Attack"))
				{
						_holdAttack = true;
						Hero.instance.Motor._lastAttackTime = Time.fixedTime;
				}

				if (CrossPlatformInputManager.GetButtonUp("Attack"))
				{
						_holdAttack = false;
						Hero.instance.Motor.Attack();
				}
				/*
				float deltaRoll = Time.fixedTime - Hero.instance.Motor._lastRollTime;
				if (CrossPlatformInputManager.GetButtonDown("Roll") && deltaRoll > Hero.instance.Manager._DeltaRoll)
						Hero.instance.Motor.Roll();*/


				if (CrossPlatformInputManager.GetButtonDown("Jump"))
				{
						jump = true;
						Hero.instance.Motor._anima.SetBool("IsFly", true);
						Hero.instance.Motor._anima.SetTrigger("IsJumping");
				}

				float timeDelta = Time.time - _lastBlockClickTime;
				if (CrossPlatformInputManager.GetButtonDown("Block"))
				{
						_holdBlock = true;
						_lastBlockClickTime = Time.fixedTime;
						if (timeDelta < _catchTime)
						{
								_doubleBlock = true;
								_holdBlock = false;
						}
				}

				if (_holdBlock && timeDelta >= _catchTime)
				{
						Hero.instance.Motor.SetBlock();
						_holdBlock = false;
				}

				if (CrossPlatformInputManager.GetButtonUp("Block"))
						Hero.instance.Motor.UnSetBlock();


				if (_doubleBlock)
				{
						float deltaBackSlide = Time.fixedTime - Hero.instance.Motor._lastDodgeTime;
						if (deltaBackSlide > Hero.instance.Manager._DeltaBack_Slide)
								Hero.instance.Motor.StartDodge();
						_doubleBlock = false;
				}
		}

		void ResetStats()
		{
				Hero.instance.Manager._TakeDamage = false;
				/*	if (Hero.instance.Motor._attacks)
					{
							Hero.instance.Motor.ResetAttack();
					}*/
		}
}
