using System;
using System.Collections;
using UnityEngine;

public enum GhostSounds
{
		None,
		Move,
		Strike,
		Death,
		When_Hit
}

[RequireComponent(typeof(GhostManager))]
public class GhostMotor : MonoBehaviour
{
		public float _speed = 10f;

		[SerializeField]
		float _distanceAttackOffset = 10f;

		[SerializeField]
		float _attackTimeOffset = 10f;

		public float _TimeHand = 1f;


		enum GhostState
		{
				Idle,
				Moves,
				Attack,
				Teleport
		}

		Animator _anima;
		Rigidbody2D _rigidbody;
		AudioManager _audioManager;

		[NonSerialized]
		public bool _reciveDamage = false;

		[NonSerialized]
		public bool _attacks = false;
		protected float _moveLeftSide = -1; //1- вправо ; -1 влево

		MyFSM _fsm;
		GhostManager _manager;

		[NonSerialized]
		public bool _visibleHero = false;

		float _currentHorAxis;
		public float CurrentHorAxis
		{
				get
				{
						return _currentHorAxis;
				}
				set
				{
						_currentHorAxis = value;
				}
		}

		// Use this for initialization
		void Start()
		{
				_anima = GetComponent<Animator>();
				_rigidbody = GetComponent<Rigidbody2D>();
				_audioManager = GetComponent<AudioManager>();
				_manager = GetComponent<GhostManager>();

				_fsm = new MyFSM(Moves);

				_fsm.AddStates((int)GhostState.Attack, DoAttack);
		}


		void Moves()
		{
				if (_manager._death)
						return;

				_attacks = false;
				_manager.ResetEnemyDealAttack();
				_anima.SetBool("Attack", false);
				_anima.SetBool("Move", _currentHorAxis != 0);

				if (!_audioManager.IsPlaying(GhostSounds.Move.ToString()) && _currentHorAxis != 0)
						_audioManager.Play(GhostSounds.Move.ToString());
				else if (_currentHorAxis == 0)
						_audioManager.Stop(GhostSounds.Move.ToString());

				_rigidbody.velocity = new Vector2(_currentHorAxis * _speed, _rigidbody.velocity.y);

				bool actionRight = false;
				if (transform.localScale.x < 0)
						actionRight = true;

				if (_visibleHero)
				{
						if (Hero.instance.transform.position.x - transform.position.x > 0 && !actionRight)
								Flip();
						else if (Hero.instance.transform.position.x - transform.position.x < 0 && actionRight)
								Flip();
				}
				else
				{
						if (_currentHorAxis > 0 && !actionRight)
								Flip();
						else if (_currentHorAxis < 0 && actionRight)
								Flip();
				}
		}


		public void Attack()
		{
				_fsm.RunState((int)GhostState.Attack);
				if (_fsm.GetCurrentState() == Moves)
						_fsm.FinishState();
		}
		
		public void ResetAllState()
		{
				_fsm.FinishAllStates();
                StopAllCoroutines();
		}

		public void ResetState()
		{
				_fsm.FinishState();
		}

		void DoAttack()
		{
				if (!_audioManager.IsPlaying(GhostSounds.Strike.ToString()))
						_audioManager.Play(GhostSounds.Strike.ToString());

				if (!_attacks)
				{						
						StartCoroutine(DoAttackCoroutine(_attackTimeOffset));
				}

				_anima.SetBool("Attack", true);
		}

		private void FixedUpdate()
		{
				if (_manager._HP >= 0)
				{
						_fsm.Invoke();
				}                
		}

		protected void Flip()
		{
				Vector3 theScale = transform.localScale;
				theScale.x *= -1;
				transform.localScale = theScale;
		}


		private IEnumerator DoAttackCoroutine(float time)
		{
				_attacks = true;

				yield return new WaitForSeconds(_TimeHand);

				Vector3 _targetAttack;
				_targetAttack = transform.position;
				_targetAttack.x += Hero.instance.transform.position.x <= transform.position.x ? -_distanceAttackOffset : _distanceAttackOffset;

				Vector3 startPosition = transform.position;
				float startTime = Time.realtimeSinceStartup;
				float fraction = 0f;

				while (fraction < 1f)
				{
						fraction = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / time);
						transform.position = Vector3.Lerp(startPosition, _targetAttack, fraction);
						yield return null;
				}
		}

}
