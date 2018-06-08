
using UnityEngine;
using System;
using System.Collections;

public class HeroMotor : CharacterMotor
{

		[NonSerialized]
		public bool _attacks = false;

		[NonSerialized]
		public bool _holdAttack = false;

		[NonSerialized]
		public float _lastAttackTime = 0;

		[NonSerialized]
		public bool _blocking = false;

		#region Roll
		[NonSerialized]
		public bool _rolling = false;
		[NonSerialized]
		public float _lastRollTime = 0;

		[SerializeField]
		float _rollLength = 20F;
		#endregion

		#region Rapira
		[SerializeField]
		public float _deltaRapiraTime = 1.5f;
		[SerializeField]
		public float _deltaRapiraLength = 1.5f;
		#endregion

		#region Back Slide
		[SerializeField]
		float _backSlideLength = 5f;
		[NonSerialized]
		public float _lastBack_SlideTime = 0;
		#endregion



		public void Attack()
		{
				_attacks = true;
				if (_deltaRapiraTime > Time.fixedTime - _lastAttackTime)
				{
						_lastAttackTime = Time.fixedTime;
						_anima.SetTrigger("Attack");
						_anima.SetInteger("Attack Index", 1);
				}
				else
				{
						_anima.SetTrigger("Rapira");
						Debug.Log("Rapira: " + (_acingRight ? 1 : -1 * _deltaRapiraLength));
						_rigidbody.AddForce(new Vector2(_acingRight ? 1 : -1 * _deltaRapiraLength, 0f), ForceMode2D.Impulse);
						//	_rigidbody.velocity = new Vector2(_acingRight ? 1 : -1 * _deltaRapiraLength, 0f);
				}
		}

		protected override bool CanMove()
		{
				return (!_attacks && !_rolling && !_blocking);
		}

		public void ResetAttack()
		{
				_anima.SetInteger("Attack Index", 0);
				_attacks = false;
		}


		public void SetBlock()
		{
				_blocking = true;
				_anima.SetTrigger("Block");
				_anima.SetBool("Blocking", _blocking);
		}

		public void UnSetBlock()
		{
				_blocking = false;
				_anima.SetBool("Blocking", _blocking);
		}

		public void Back_Slide()
		{
				_attacks = true;
				_lastBack_SlideTime = Time.fixedTime;
				_blocking = false;
				_anima.SetTrigger("Back_Slide");
				_rigidbody.AddForce(new Vector2(-_backSlideLength * transform.localScale.x, 0f), ForceMode2D.Impulse);
		}



		public void Roll()
		{
				_rolling = true;
				_lastRollTime = Time.fixedTime;
				_anima.SetTrigger("Roll");
				//_rigidbody.velocity = new Vector2(_rollLength * transform.localScale.x, 1f);
				_rigidbody.AddForce(new Vector2(_rollLength * transform.localScale.x, 0f), ForceMode2D.Impulse);
		}
}
