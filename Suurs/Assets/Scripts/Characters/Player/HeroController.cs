﻿using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(HeroManager))]
public class HeroController : Unit
{

		[SerializeField]
		float _rollLength = 20F;

		[SerializeField]
		float _recoilLength = 5f;

		Rigidbody2D _rigidbody;

		[NonSerialized]
		public Animator _anima;

		[NonSerialized]
		public int _comboAttack = 1;

		float _lastAttackTime = 0;
		float _catchTime = .17f;
		bool _doubleAttack = false;
		bool _acingRight = true;

		[NonSerialized]
		public bool _jumping = false;
		float _lastJumpTime = 0;

		[NonSerialized]
		public bool _attacks = false;
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
		GlobalHealthController _GlobalHealthController;

		Vector3 _offset;
		bool moveRecoil = false;


		private void Start()
		{
				_rigidbody = GetComponent<Rigidbody2D>();
				_anima = GetComponent<Animator>();
				_manager = GetComponent<HeroManager>();
				_lastJumpTime = Time.fixedTime;
				_GlobalHealthController = GameObject.FindGameObjectWithTag("GlobalHealth").GetComponent<GlobalHealthController>();
		}


		public void Move(float Axis, GameObject inTarget)
		{
				transform.position = Vector2.MoveTowards(transform.position, inTarget.transform.position, Axis * _speed * Time.fixedDeltaTime);
				_anima.SetFloat("Speed", Axis);
		}


		private void FixedUpdate()
		{
				if (_manager._HP <= 0)
						return;

				if (_interfaceBlocked)
						return;

				if (Time.fixedTime - _lastAttackTime > 2f)
						_comboAttack = 1;

				if (_comboAttack > 2)
						_comboAttack = 2;

				if (!_attacks && !_jumping && !_blocking)
				{
						float h = CrossPlatformInputManager.GetAxis("Horizontal");
						Move(_rigidbody, _speed, ref _acingRight, h);
						_anima.SetFloat("Speed", Mathf.Abs(h));
				}

				if (CrossPlatformInputManager.GetButtonDown("Attack"))
				{
						Debug.Log(_jumping);
						if (!_jumping)
								Attack(_comboAttack);
						else
								Attack(3);
				}

				int deltaJump = (int)Math.Truncate((Time.fixedTime - _lastJumpTime) * 1000);
				if (CrossPlatformInputManager.GetButtonDown("Jump") && !_attacks && (deltaJump > _manager._DeltaRoll))
						Jump();


				CheckBlock();
				if (_doubleBlock && !moveRecoil)
				{
						moveRecoil = true;
						_blocking = false;
						_anima.SetTrigger("Recoil");
						_offset = new Vector3(transform.position.x - _recoilLength * transform.localScale.x, transform.position.y, transform.position.z);
						//_rigidbody.velocity = new Vector2(_recoilLength * transform.localScale.x, 1);
						_doubleBlock = false;
						_lastDoubleBlockClickTime = Time.fixedTime;
				}

				if (moveRecoil)
				{
						transform.position = Vector3.Lerp(transform.position, _offset, _recoilLength * Time.fixedDeltaTime);
						if (Math.Round(transform.position.x, 1) == Math.Round(_offset.x, 1) || Time.fixedTime - _lastDoubleBlockClickTime > .4f) 
						{
								Debug.Log("все ок");
								moveRecoil = false;
						}
				}

		}

		private void CheckBlock()
		{
				float timeDelta = Time.time - _lastBlockClickTime;
				if (CrossPlatformInputManager.GetButtonDown("Block"))
				{
						_holdBlock = true;
						_lastBlockClickTime = Time.time;
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

		public void DisableAnima()
		{
				_anima.enabled = false;
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
		private void Jump()
		{
				_jumping = true;
				_lastJumpTime = Time.fixedTime;
				_anima.SetFloat("Speed", 0);
				_anima.SetTrigger("Jump");
				_rigidbody.velocity = new Vector2(_rollLength * transform.localScale.x, 1);
		}

		void ResetStats()
		{
				_jumping = false;

				if (_attacks)
				{
						_manager.ResetHeroDealAttack();
						_anima.SetFloat("Attack", 0);
						_attacks = false;
				}
		}

		private void Attack(float inTypeAttack)
		{
				_attacks = true;
				_anima.SetFloat("Speed", 0);
				_anima.SetFloat("Attack", inTypeAttack);
				_lastAttackTime = Time.fixedTime;

		}

		public void ResetAttack()
		{
				if (_attacks)
				{
						_manager.ResetHeroDealAttack();
						_anima.SetFloat("Attack", 0);
						_attacks = false;
				}
		}

		public void ResetJumping()
		{
				_jumping = false;
		}

}
