using System.Collections;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class HeroManager : MonoBehaviour
{

		public int _Level = 1; ///уровень

		public float _MaxHealth = 100; ///Максимальная жизнь	
		public float _Health = 100; ///Текущая жизнь

		public float _MaxEnergy = 100; ///Максимальная энергия	
		public float _Energy = 100; ///Текущая энергия

		public float _GlobalMaxHealth = 100; ///Максимальная глобальная жизнь
		public float _GlobalHealth = 100; ///Текущая глобальная жизнь

		public float _attack = 25; ///атака
		public float _Shield = 50f; ///Щит   
		public float _Protaction = 0f; ///Защита 
		public float _SpeedAttack = 100f; ///Скорость атаки
		public float _Agility = 0; ///Ловкость
		public float _Power = 0; ///Сила
		public float _Vitality = 0; ///Жизнеспособность
		public int _DeltaRoll = 2000; ///Интервал кувырков (в милисекундах) 
		public int _DeltaBack_Slide = 1000; ///Интервал Back_Slide (в милисекундах) 


		//public Hero heroStat;
		string path;

		Animator _anima;
		bool _death = false;

		bool _DealDamage = false;
		public List<AttackItem> _attackItems;

		private void Start()
		{
				_anima = GetComponent<Animator>();
		}

		private void Update()
		{
				if (_Health <= 0 && !_death)
				{
						_death = true;
						_anima.SetTrigger("Death");
						Die();
				}
		}

		public virtual void Die()
		{
				GetComponent<Rigidbody2D>().gravityScale = 1;
				Invoke("DestroyObject", 4f);
				PlayerPrefs.SetInt("NextLVL", 0);
		}

		protected virtual void DestroyObject()
		{
				SceneManager.LoadScene("Loading");
				//Destroy(transform.root.gameObject);
		}

		void OnTriggerEnter2D(Collider2D collision)
		{
				Debug.Log("Атака");
				if (collision.tag == "Enemy" && Hero.instance.Motor._attacks)
				{
						Debug.Log(Hero.instance.Motor.SwordCollider.enabled);
						if (Hero.instance.Motor.SwordCollider.enabled)
						{
								Hero.instance.Motor.SwordCollider.enabled = false;
								var _currentAttackItem = _attackItems.Where(x => x._ID == Hero.instance.Motor._attacksIndex).FirstOrDefault();
								GameObject enemy = collision.transform.root.gameObject;
								EnemyController enemyController = enemy.GetComponent<EnemyController>();
								enemyController.TakeHit(_currentAttackItem._damage + (_attack / 100 * _currentAttackItem._damage));
						}
				}
		}

		public void ResetHeroDealAttack()
		{
				_DealDamage = false;
		}

		private void OnGUI()
		{
				string boxText =
					"Level = " + _Level + "\n" +
					"Attack = " + _attack + "\n" +
					"Agility = " + _Agility + "\n";
				GUI.Box(new Rect(0, 0, 150, 100), boxText);
		}

		void AddAgility()
		{
				if (_Agility == 100)
						return;

				_SpeedAttack += _SpeedAttack * 0.002f;
				_DeltaRoll -= 100;
				_Agility++;
				if ((_Agility != 0) && (_Agility % 7 == 0))
						_SpeedAttack += _SpeedAttack * 0.02f;
				if ((_Agility != 0) && (_Agility % 100 == 0))
						_SpeedAttack += _SpeedAttack * 0.02f;

		}
		void AddPower()
		{
				if (_Power == 100)
						return;

				_attack += 2;
				_MaxHealth += 2;
				_Power++;
				if ((_Power != 0) && (_Power % 10 == 0))
				{
						_attack += 10 + (int)(_Power / 10) * 10;
						_MaxHealth += 10;
				}
				if ((_Agility != 0) && (_Agility % 100 == 0))
						_SpeedAttack += _SpeedAttack * 0.02f;

		}
		void AddVitality()
		{
				if (_Vitality == 100)
						return;

				_MaxHealth += 10;
				_Shield += 2f;
				_Vitality++;
				if ((_Vitality != 0) && (_Vitality % 10 == 0))
				{
						_MaxHealth += 10 + (int)(_Vitality / 10) * 10;
						_Protaction += 5f;
				}
				/*
						if ((_Agility != 0) && (_Agility % 100 == 0))
								_SpeedAttack += _SpeedAttack * 0.02f;*/
		}

		void Load()
		{
				//PlayerPrefs.
		}
}
