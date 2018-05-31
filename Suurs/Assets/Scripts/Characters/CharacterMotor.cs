using System.Collections;
using System;
using UnityEngine;

public class CharacterMotor : MonoBehaviour
{
		[SerializeField]
		float _speed = 16f;

		protected Rigidbody2D _rigidbody;

		[NonSerialized]
		public Animator _anima;


		bool _acingRight = true;

		// Use this for initialization
		void Start()
		{
				_rigidbody = GetComponent<Rigidbody2D>();
				_anima = GetComponent<Animator>();
		}


		public virtual void Move(float inMoveDirection)
		{
				_rigidbody.velocity = new Vector2(inMoveDirection * _speed, _rigidbody.velocity.y);
				_anima.SetFloat("Speed", Mathf.Abs(inMoveDirection));

				if (inMoveDirection > 0 && !_acingRight)
						Flip();
				else if (inMoveDirection < 0 && _acingRight)
						Flip();
		}

		protected void Flip()
		{
				// Switch the way the player is labelled as facing.
				_acingRight = !_acingRight;

				// Multiply the player's x local scale by -1.
				Vector3 theScale = transform.root.localScale;
				theScale.x *= -1;
				transform.root.localScale = theScale;
		}
}
