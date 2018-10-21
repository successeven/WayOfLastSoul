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

		public bool _interfaceBlocked = true;
		bool jump = false;

		void FixedUpdate()
		{

				if (Hero.instance.Manager._Health <= 0)
						return;

				if (_interfaceBlocked)
						return;


				if (_holdAttack)
				{
						float deltaPower = (Time.fixedTime - Hero.instance.Motor._lastAttackTime) / Hero.instance.Motor._deltaShield_AttackTime;

						Hero.instance.Motor._anima.SetFloat("Shield Power", Math.Min(deltaPower, 1));
				}
				else
						Hero.instance.Motor._anima.SetFloat("Shield Power", 0);

				Hero.instance.Motor.CurrentHorAxis = CrossPlatformInputManager.GetAxis("Horizontal");

		}

		float _timeAttackHold;
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

				if (CrossPlatformInputManager.GetButtonDown("Jump") && Hero.instance.Motor.m_Grounded)
						Hero.instance.Motor.Jump();
				
				if (CrossPlatformInputManager.GetButtonDown("Uppercut"))
						Hero.instance.Motor.Uppercut();

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
						Hero.instance.Motor.Dodge();
						_doubleBlock = false;
				}
		}

		void ResetStats()
		{
				Hero.instance.Manager._TakeDamage = false;
		}
}
