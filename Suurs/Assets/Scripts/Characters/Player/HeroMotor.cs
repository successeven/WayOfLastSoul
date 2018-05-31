
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


		private void Roll()
		{
				_rolling = true;
				_lastRollTime = Time.fixedTime;
				_anima.SetTrigger("Roll");
				_rigidbody.AddForce(new Vector2(_rollLength * transform.localScale.x, 1f));
		}
}
