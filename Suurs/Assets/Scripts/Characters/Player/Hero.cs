using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(HeroMotor))]
[RequireComponent(typeof(HeroManager))]
public class Hero : MonoBehaviour
{

		#region Singleton

		public static Hero instance;

		void Awake()
		{
				instance = this;
		}

		#endregion


		HeroManager _heroManager;
		HeroMotor _heroMotor;
		// Use this for initialization
		void Start()
		{
				_heroManager = GetComponent<HeroManager>();
				_heroMotor = GetComponent<HeroMotor>();
		}

		public void Move(float inSpeed)
		{
				_heroMotor.Move(inSpeed);
		}

		public void TakeDamage(float damage) //Урон
		{
				if (_heroMotor._blocking)
				{
						_heroManager._Health -= damage * (_heroManager._Shield / 100);
						_heroMotor._anima.SetTrigger("TakeHitWhenBlocking");
				}
				else
				{
						_heroManager._Health -= damage;
						_heroMotor._anima.SetTrigger("TakeHit");
				}
		}
		// Update is called once per frame
		void Update()
		{

		}
}
