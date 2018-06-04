
using UnityEngine;
using System;

public class HeroMotor : CharacterMotor
{
		[NonSerialized]
		public bool _rolling = false;
		[NonSerialized]
		public float _lastRollTime = 0;

		[SerializeField]
		float _rollLength = 20F;

		[NonSerialized]
		public bool _attacks = false;

		[NonSerialized]
		public bool _holdAttack = false;

		[NonSerialized]
		public float _lastAttackTime = 0;

		[SerializeField]
		public float _deltaRapiraTime = 1.5f;
		[SerializeField]
		public float _deltaRapiraLength = 1.5f;


		[NonSerialized]
		public bool _blocking = false;
		/*
		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}
		*/

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
						_rigidbody.AddForce(new Vector2(_acingRight ? 1 : -1 * _deltaRapiraLength, 0));
				}
		}

		public void ResetAttack()
		{
				_anima.SetInteger("Attack Index", 0);
				_attacks = false;
		}


		public void Roll()
		{
				_rolling = true;
				_lastRollTime = Time.fixedTime;
				_anima.SetTrigger("Roll");
				_rigidbody.velocity = new Vector2(_rollLength * transform.localScale.x, 1f);
				//_rigidbody.AddForce(new Vector2(_rollLength * transform.localScale.x, 1f));
		}
}
