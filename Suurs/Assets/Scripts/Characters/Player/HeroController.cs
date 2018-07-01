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
		HeroManager _manager;
		HeroMotor _motor;

		
		private void Start()
		{
				_manager = GetComponent<HeroManager>();
				_motor = GetComponent<HeroMotor>();
		}

		void FixedUpdate()
		{

				if (_manager._Health <= 0)
						return;

				if (_interfaceBlocked)
						return;


				_motor.Move(CrossPlatformInputManager.GetAxis("Horizontal"));

		}

		private void Update()
		{
				if (_manager._Health <= 0)
						return;

				if (_interfaceBlocked)
						return;
				
				if (CrossPlatformInputManager.GetButtonDown("Attack"))
				{
						_holdAttack = true;
						_motor._lastAttackTime = Time.fixedTime;
				}

				if (CrossPlatformInputManager.GetButtonUp("Attack"))
				{
						_manager.ResetHeroDealAttack();
						_holdAttack = false;
						_motor.Attack();
				}

				int deltaRoll = (int)Math.Truncate((Time.fixedTime - _motor._lastRollTime) * 1000);
				if (CrossPlatformInputManager.GetButtonDown("Roll") && deltaRoll > _manager._DeltaRoll)
						_motor.Roll();
				
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
						_motor.SetBlock();
						_holdBlock = false;
				}

				if (CrossPlatformInputManager.GetButtonUp("Block"))
						_motor.UnSetBlock();


				if (_doubleBlock)
				{
						_motor.Back_Slide();
						_doubleBlock = false;
				}
		}

		void ResetStats()
		{
				if (_motor._attacks)
				{
						_motor.ResetAttack();
				}
		}
}
