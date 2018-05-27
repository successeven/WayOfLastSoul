using System.Collections;
using System;
using UnityEngine;

public class EnemyController : Unit
{

	[SerializeField]
	protected float _visibility = 25;


	[SerializeField]
	protected int _deltaTimeAttack = 2000;

	[SerializeField]
	protected float _deltaDistanceAttack = 2f;

	protected GameObject _player;
	protected HeroManager _playerManager;
	protected HeroController _playerController;
	protected Animator _anima;
	protected EnemyManager _enemyManager;
	protected Rigidbody2D _rigidbody;



	protected bool _moving = false;
	protected bool _reciveDamage = false;
	[NonSerialized]
	public bool _attacks = false;
	protected float _lastAttackTime;
	protected float _distance;

	protected float _moveLeftSide = -1; //1- вправо ; -1 влево

	void Start()
	{
		_player = GameObject.FindGameObjectWithTag("Player");
		_playerManager = _player.GetComponent<HeroManager>();
		_playerController = _player.GetComponent<HeroController>();
		_anima = GetComponent<Animator>();

		/*if (_anima == null)
                _anima = GetComponentInChildren<Animator>();*/

		_enemyManager = GetComponent<EnemyManager>();
		_rigidbody = transform.root.GetComponent<Rigidbody2D>();
		AfterStart();
	}

	protected virtual void AfterStart()
	{
	}

	void Update()
	{
		if (Vector3.Distance(_player.transform.position, transform.position) > 50)
			_anima.enabled = false;
		else
			_anima.enabled = true;


		if (_playerManager._HP <= 0)
		{
			_anima.SetBool("Move", false);
			return;
		}
	}

	protected virtual void FixedUpdate()
	{
		if (_enemyManager._HP <= 0 || _playerManager._HP <= 0)
			return;

		DoMotion();

		DoAttack();
	}


	protected virtual void DoMotion()
	{
	}

	public virtual void TakeHit()
	{
	}

	protected virtual void DoAttack()
	{
	}
}
