using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(HeroMotor))]
[RequireComponent(typeof(HeroManager))]
[RequireComponent(typeof(HeroController))]
public class Hero : MonoBehaviour
{

		#region Singleton

		public static Hero instance;

		void Awake()
		{
				instance = this;
				_heroController = GetComponent<HeroController>();
				_heroManager = GetComponent<HeroManager>();
				_heroMotor = GetComponent<HeroMotor>();
		}

		#endregion

		HeroController _heroController;
		public HeroController Controller
		{
				get
				{
						return _heroController;
				}
		}

		HeroManager _heroManager;
		public HeroManager Manager
		{
				get
				{
						return _heroManager;
				}
		}


		HeroMotor _heroMotor;
		public HeroMotor Motor
		{
				get
				{
						return _heroMotor;
				}
		}

		public AudioSource audioSource
		{
				get
				{
						return GetComponent<AudioSource>();
				}
		}

		public const string GameOverAD = "ca-app-pub-4537576181628162/5928041347";
		public const string bannerAD = "ca-app-pub-4537576181628162/5626457313";

		public void Move(float inSpeed)
		{
				_heroMotor.Move(inSpeed);
		}

		public void TakeDamage(float damage) //Урон
		{

				if (_heroMotor._blocking)
				{
						audioSource.clip = Hero.instance.Manager._BlockSound;
						_heroManager._Health -= damage * (_heroManager._Shield / 100);
						_heroMotor._anima.SetTrigger("TakeHitWhenBlocking");
				}
				else
				{
						audioSource.clip = Hero.instance.Manager._HitSound;
						_heroManager._Health -= damage;
						_heroMotor._anima.SetTrigger("TakeHit");
				}
				audioSource.Play();
		}
		// Update is called once per frame
		void Update()
		{

		}
}
