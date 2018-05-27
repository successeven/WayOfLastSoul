using System;
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
	float _currentSpeed = 0;

	[SerializeField]
	public float _deltaRapiraTime = 1.5f;
	[SerializeField]
	public float _deltaRapiraLength = 1.5f;

	[NonSerialized]
	public Animator _anima;

	[NonSerialized]
	public int _comboAttack = 1;
	
	[NonSerialized]
	public float _lastAttackTime = 0;
	float _catchTime = .17f;
	bool _doubleAttack = false;
	bool _acingRight = true;

	[NonSerialized]
	public bool _rolling = false;
	[NonSerialized]
	public float _lastRollTime = 0;

	[NonSerialized]
	public bool _attacks = false;
	[NonSerialized]
	public bool _holdAttack = false;


	
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

	Vector3 _offset;
	bool moveRecoil = false;


	private void Start()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_anima = GetComponent<Animator>();
		_manager = GetComponent<HeroManager>();
		_lastRollTime = Time.fixedTime;
	}

	public void Move(float inSpeed)
	{
		Move(_rigidbody, _speed, ref _acingRight, inSpeed);
		_anima.SetFloat("Speed", Mathf.Abs(inSpeed));
	}

	void FixedUpdate()
	{

		if (_manager._HP <= 0)
			return;

		if (_interfaceBlocked)
			return;


		if (!_attacks && !_rolling && !_blocking)
		{
			_currentSpeed = CrossPlatformInputManager.GetAxis("Horizontal");
			Move(_rigidbody, _speed, ref _acingRight, _currentSpeed);
			_anima.SetFloat("Speed", Mathf.Abs(_currentSpeed));
		}

	}

	private void Update()
	{
		if (_manager._HP <= 0)
			return;

		if (_interfaceBlocked)
			return;

		if (CrossPlatformInputManager.GetButtonDown("Attack"))
		{
			_holdAttack = true;
			_lastAttackTime = Time.fixedTime;
		}


		if (CrossPlatformInputManager.GetButtonUp("Attack"))
		{
			_holdAttack = false;
			_attacks = true;
			if (_deltaRapiraTime > Time.fixedTime - _lastAttackTime)
			{
				_lastAttackTime = Time.fixedTime;
				_anima.SetTrigger("Attack");
			}
			else
			{
				_anima.SetTrigger("Rapira");
				_rigidbody.AddForce ( new Vector2(_acingRight ? 1: -1 * _deltaRapiraLength, 0));
			}
		}


			int deltaRoll = (int)Math.Truncate((Time.fixedTime - _lastRollTime) * 1000);
		if (CrossPlatformInputManager.GetButtonDown("Roll") && (deltaRoll > _manager._DeltaRoll))
			Roll();


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

	}

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

	private void Roll()
	{
		_rolling = true;
		_lastRollTime = Time.fixedTime;
		_anima.SetTrigger("Roll");
		_rigidbody.AddForce(new Vector2(_rollLength * transform.localScale.x, 1f));
	}

	void ResetStats()
	{
		_rolling = false;
		if (_attacks)
		{
			_manager.ResetHeroDealAttack();
			_anima.SetFloat("Attack", 0);
			_attacks = false;
		}
	}

}
