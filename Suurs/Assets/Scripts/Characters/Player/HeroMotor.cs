
using UnityEngine;
using System;
using System.Collections;

public class HeroMotor : CharacterMotor
{

		[NonSerialized]
		public bool _attacks = false;
		[SerializeField]
		float _attacksLength = 2f;
		[NonSerialized]
		public int _attacksIndex = 0;

		[NonSerialized]
		public bool _holdAttack = false;

		[NonSerialized]
		public float _lastAttackTime = 0;

		[NonSerialized]
		public bool _blocking = false;

		public bool _canMove = true;
		

		#region Roll
		[NonSerialized]
		public bool _rolling = false;
		[NonSerialized]
		public float _lastRollTime = 0;

		[SerializeField]
		float _rollLength = 20f;
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
				
				if (_rolling)
				{
						_attacksIndex = 3;
						_anima.SetTrigger("Attack");
						_anima.SetInteger("Attack Index", _attacksIndex);
						StartCoroutine(DoStrikeRoll(.33f));
				}
				else if (_deltaRapiraTime > Time.fixedTime - _lastAttackTime)
				{
						_attacksIndex = _attacksIndex == 0 ? 3 : _attacksIndex;
						Debug.Log(_attacksIndex);
						_lastAttackTime = Time.fixedTime;
						_attacksIndex++;
						_anima.SetTrigger("Attack");
						_anima.SetInteger("Attack Index", _attacksIndex);
						StartCoroutine(DoAttack(.4f));
				}
				else
				{
						_attacksIndex = 2;
						_anima.SetTrigger("Rapira");
						StartCoroutine(DoRapira(.33f));
				}
		}
		private IEnumerator DoStrikeRoll(float time)
		{
				for (float t = 0; t <= time; t += Time.deltaTime)
						yield return null;
				ResetAttack();
		}

		private IEnumerator DoAttack(float time)
		{
				for (float t = 0; t <= time; t += Time.deltaTime)
				{
						_rigidbody.velocity = new Vector2(_attacksLength * transform.localScale.x, _rigidbody.velocity.y);
						yield return null;
				}
				ResetAttack();
		}

		private IEnumerator DoRapira(float time)
		{
				for (float t = 0; t <= time; t += Time.deltaTime)
				{
						_rigidbody.velocity = new Vector2(_deltaRapiraLength * transform.localScale.x, _rigidbody.velocity.y);
						yield return null;
				}
				ResetAttack();
		}


		protected override bool CanMove()
		{
				return (!_attacks && !_rolling && !_blocking && _canMove);
		}

		public void ResetAttack()
		{
				_attacksIndex = 0;
				_anima.SetInteger("Attack Index", _attacksIndex);
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
				_attacksIndex = 1;
				_attacks = true;
				_lastBack_SlideTime = Time.fixedTime;
				_blocking = false;
				_anima.SetTrigger("Back_Slide");
				StartCoroutine(DoBack_Slide(.45f));
		}

		private IEnumerator DoBack_Slide(float time)
		{
				for (float t = 0; t <= time; t += Time.deltaTime)
				{
						_rigidbody.velocity = new Vector2(-_backSlideLength * transform.localScale.x, _rigidbody.velocity.y);
						yield return null;
				}
				ResetAttack();
		}

		public void Roll()
		{
				_rolling = true;
				_lastRollTime = Time.fixedTime;
				_anima.SetTrigger("Roll");
				StartCoroutine(DoRolling(.6f));
		}

		private IEnumerator DoRolling(float time)
		{
				for (float t = 0; t <= time; t += Time.deltaTime)
				{
						_rigidbody.velocity = new Vector2(_rollLength * transform.localScale.x, _rigidbody.velocity.y);
						yield return null;
				}
				_rolling = false;
		}
}
