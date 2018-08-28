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


		void FixedUpdate()
		{

				if (Hero.instance.Manager._Health <= 0)
						return;

				if (_interfaceBlocked)
						return;


				Hero.instance.Motor.Move(CrossPlatformInputManager.GetAxis("Horizontal"));

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

				int deltaRoll = (int)Math.Truncate((Time.fixedTime - Hero.instance.Motor._lastRollTime) * 1000);
				if (CrossPlatformInputManager.GetButtonDown("Roll") && deltaRoll > Hero.instance.Manager._DeltaRoll)
						Hero.instance.Motor.Roll();

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
						Hero.instance.Motor.StartBack_Slide();
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
