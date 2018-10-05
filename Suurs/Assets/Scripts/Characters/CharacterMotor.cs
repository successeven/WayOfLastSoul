﻿using System.Collections;
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

		public float k_GroundedRadius = .4f;

		protected Rigidbody2D _rigidbody;

		[NonSerialized]
		public Animator _anima;

		protected bool _acingRight = true;
		public bool m_Grounded;            // Whether or not the player is grounded.

		[NonSerialized]
		public float _currentSpeed;

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
				bool wasGrounded = m_Grounded;
				m_Grounded = false;
				Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);

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

		public virtual void Move(float inMoveDirection)
		{

				_anima.SetFloat("Speed", Mathf.Abs(inMoveDirection));
				if (CanMove() && m_Grounded)
				{
						if (!Hero.instance.Manager._TakeDamage)
								if (inMoveDirection != 0 && !Hero.instance.audioManager.IsPlaying(Hero.AudioClips.Run.ToString()))
										Hero.instance.audioManager.Play(Hero.AudioClips.Run.ToString());
								else if (inMoveDirection == 0)
										Hero.instance.audioManager.Stop(Hero.AudioClips.Run.ToString());

						Hero.instance.audioManager.SetPitch(Hero.AudioClips.Run.ToString(), Math.Abs(inMoveDirection));

						_currentSpeed = inMoveDirection * _speed;
						//_rigidbody.velocity = new Vector2(inMoveDirection * _speed, _rigidbody.velocity.y);

						Vector3 targetVelocity = new Vector2(inMoveDirection * _speed, _rigidbody.velocity.y);
						_rigidbody.velocity = Vector3.SmoothDamp(_rigidbody.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

						if (inMoveDirection > 0 && !_acingRight)
								Flip();
						else if (inMoveDirection < 0 && _acingRight)
								Flip();

				}
				else
				{
						_currentSpeed = 0;
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
