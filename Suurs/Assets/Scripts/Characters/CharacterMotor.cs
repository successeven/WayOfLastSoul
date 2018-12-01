using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Events;

public class CharacterMotor : MonoBehaviour
{
		[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
		[SerializeField] protected float _JumpForce = 400f;              // Amount of force added when the player jumps.
		[SerializeField] private LayerMask m_WhatIsGround;              // A mask determining what is ground to the character
		[SerializeField] private Transform m_GroundCheck;             // A position marking where to check if the player is grounded.
		[SerializeField] private float _speed = 16f;

		public float k_GroundedRadius = 1f;

		protected Rigidbody2D _rigidbody;

		[NonSerialized]
		public Animator _anima;

		protected bool _acingRight = true;

		[HideInInspector]
		public bool m_Grounded;            // Whether or not the player is grounded.

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

		private Vector3 m_Velocity = Vector3.zero;


		[Header("Events")]
		[Space]
		public UnityEvent OnLandEvent;


		// Use this for initialization
		private void Awake()
		{
				_rigidbody = GetComponent<Rigidbody2D>();
				_anima = GetComponent<Animator>();
		}
		
		protected virtual bool CanMove()
		{
				return true;
		}

		protected void CheckGround()
		{
				m_Grounded = false;
				Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
				_anima.SetFloat("v_speed", _rigidbody.velocity.y);
				for (int i = 0; i < colliders.Length; i++)
				{
						if (colliders[i].gameObject != gameObject)
						{
								m_Grounded = true;
								break;
						}
				}
				OnLandEvent.Invoke();
		}

		public virtual void Move()
		{

				_anima.SetFloat("Speed", Mathf.Abs(_currentHorAxis));
				if (CanMove() && m_Grounded)
				{
						if (!Hero.instance.Manager._TakeDamage)
								if (_currentHorAxis != 0)
                                {
                                    if (_currentHorAxis < 0.8f)
                                    {
                                        if (!Hero.instance.audioManager.IsPlaying(Hero.AudioClips.Walk.ToString()))
                                        {
                                            Hero.instance.audioManager.Stop(Hero.AudioClips.Run.ToString());
										    Hero.instance.audioManager.Play(Hero.AudioClips.Walk.ToString());
                                        }
                                    }
                                    else
                                    {
                                        if (!Hero.instance.audioManager.IsPlaying(Hero.AudioClips.Run.ToString()))
                                        {                                            
										    Hero.instance.audioManager.Stop(Hero.AudioClips.Walk.ToString());
                                            Hero.instance.audioManager.Play(Hero.AudioClips.Run.ToString());
                                        }
                                    }
                                } 
								else 
                                {
                                    Hero.instance.audioManager.Stop(Hero.AudioClips.Run.ToString());
                                    Hero.instance.audioManager.Stop(Hero.AudioClips.Walk.ToString());
                                }

						Hero.instance.audioManager.SetPitch(Hero.AudioClips.Run.ToString(), Math.Abs(_currentHorAxis));

						Vector3 targetVelocity = new Vector2(_currentHorAxis * _speed, _rigidbody.velocity.y);
						_rigidbody.velocity = Vector3.SmoothDamp(_rigidbody.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

						if (_currentHorAxis > 0 && !_acingRight)
								Flip();
						else if (_currentHorAxis < 0 && _acingRight)
								Flip();

				}
		}

		public void Flip()
		{
				// Switch the way the player is labelled as facing.
				_acingRight = !_acingRight;

				// Multiply the player's x local scale by -1.
				Vector3 theScale = transform.root.localScale;
				theScale.x *= -1;
				transform.root.localScale = theScale;

		}
}
