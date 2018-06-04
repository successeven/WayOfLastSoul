using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(HeroManager))]
[RequireComponent(typeof(HeroMotor))]
public class HeroController : MonoBehaviour
{

		[SerializeField]
		float _recoilLength = 5f;
		
		float _catchTime = .17f;

		bool _doubleAttack = false;




		[NonSerialized]
		public bool _blocking = false;

		[NonSerialized]
		public bool _recoil = false;
		bool _isRecoil = false;

		bool _doubleBlock = false;
		float _lastDoubleBlockClickTime = 0;
		float _lastBlockClickTime = 0;
		bool _holdBlock = false;

		[NonSerialized]
		public bool _interfaceBlocked = true;
		HeroManager _manager;
		HeroMotor _motor;

		Vector3 _offset;
		bool moveRecoil = false;


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


				//	if (!_attacks && !_rolling && !_blocking)
				_motor.Move(CrossPlatformInputManager.GetAxis("Horizontal"));

		}

		private void Update()
		{
				if (_manager._Health <= 0)
						return;

				if (_interfaceBlocked)
						return;
				
				if (CrossPlatformInputManager.GetButtonDown("Attack"))
						_motor._lastAttackTime = Time.fixedTime;

				if (CrossPlatformInputManager.GetButtonUp("Attack"))
						_motor.Attack();

				int deltaRoll = (int)Math.Truncate((Time.fixedTime - _motor._lastRollTime) * 1000);
				if (CrossPlatformInputManager.GetButtonDown("Roll") && deltaRoll > _manager._DeltaRoll)
						_motor.Roll();

				/*
				CheckBlock();


				if (_doubleBlock && !moveRecoil)
				{
						moveRecoil = true;
						_blocking = false;
						_anima.SetTrigger("Back_Slide");
						_offset = new Vector3(transform.position.x - _recoilLength * transform.localScale.x, transform.position.y, transform.position.z);
						_doubleBlock = false;
						_lastDoubleBlockClickTime = Time.fixedTime;
				}

				if (moveRecoil)
				{
						transform.position = Vector3.Lerp(transform.position, _offset, _recoilLength * Time.deltaTime);
						if (Math.Round(transform.position.x, 2) == Math.Round(_offset.x, 2) || Time.fixedTime - _lastDoubleBlockClickTime > .4f)
								moveRecoil = false;
				}
				*/
		}
		/*
		private void CheckBlock()
		{
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
						SetBlock();

				if (CrossPlatformInputManager.GetButtonUp("Block"))
						UnSetBlock();
		}
		
		private void SetBlock()
		{
				_blocking = true;
				_holdBlock = false;
				_anima.SetTrigger("Block");
				_anima.SetBool("Blocking", _blocking);
		}

		private void UnSetBlock()
		{
				_blocking = false;
				_anima.SetBool("Blocking", _blocking);
		}
		
		*/

		void ResetStats()
		{
				_motor._rolling = false;
				if (_motor._attacks)
				{
						_manager.ResetHeroDealAttack();
						_motor.ResetAttack();
				}
		}
}
