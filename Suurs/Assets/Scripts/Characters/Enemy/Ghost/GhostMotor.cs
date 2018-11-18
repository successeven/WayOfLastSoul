using System;
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
		}


		void Moves()
		{
				if (_manager._death)
						return;

				_attacks = false;
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

		private void FixedUpdate()
		{
				if (_manager._HP > 0)
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
}
