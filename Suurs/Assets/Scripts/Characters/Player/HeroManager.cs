using EZCameraShake;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

		[Range(0, 100)]
		public float _Shield = 50f; ///Щит   

		[Range(0, 100)]
		public float _Protaction = 0f; ///Защита 
		public float _SpeedAttack = 100f; ///Скорость атаки
		public float _Agility = 0; ///Ловкость
		public float _Power = 0; ///Сила
		public float _Vitality = 0; ///Жизнеспособность
		public float _DeltaRoll = 2f; ///Интервал кувырков (в милисекундах) 
		public float _DeltaBack_Slide = 1f; ///Интервал Back_Slide (в милисекундах) 

		Animator _anima;
		bool _death = false;

		public List<AttackItem> _attackItems;

		[NonSerialized]
		public bool _TakeDamage = false;

		private void Start()
		{
				_anima = GetComponent<Animator>();
		}

		private void Update()
		{
				if (_Health <= 0 && !_death)
				{
						Hero.instance.audioManager.Play(Hero.AudioClips.Death.ToString());
						_death = true;
						_anima.SetTrigger("Death");
						Invoke("GameOver", 3f);
				}
		}

		public void GameOver()
		{
				UIController.instance.GameOver();
		}
		
		public void OnLanding()
		{
				_anima.SetBool("IsJumping", false);
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
				if (collision.tag == "Enemy" && Hero.instance.Motor._attacks)
				{
						GameObject enemy = collision.transform.gameObject;
						EnemyController enemyController = enemy.GetComponent<EnemyController>();
						if (enemyController == null)
						{
								Debug.LogWarning("Не найден контроллер врага!!!");
								return;
						}
						if (enemyController._reciveDamage)
								enemyController._reciveDamage = false;
				}
		}

		void OnTriggerEnter2D(Collider2D collision)
		{
				if (collision.tag == "Enemy" && Hero.instance.Motor._attacks)
				{
						GameObject enemy = collision.transform.gameObject;
						EnemyController enemyController = enemy.GetComponent<EnemyController>();
						if (enemyController == null)
						{
								Debug.LogWarning("Не найден контроллер врага!!!");
								return;
						}
						if (!enemyController._reciveDamage)
						{
								var _currentAttackItem = _attackItems.Where(x => x._ID == Hero.instance.Motor.AttackIndex).FirstOrDefault();
								enemyController.TakeHit(_currentAttackItem._damage);// + (_attack / 100 * _currentAttackItem._damage));
						}
				}
		}


		public void TakeDamage(float damage) //Урон
		{
				_TakeDamage = true;
				Hero.instance.Motor.FinishAllAttacks();
				if (Hero.instance.Motor._blocking)
				{
						Hero.instance.audioManager.Play(Hero.AudioClips.Block.ToString());
						_Health -= damage * ((100f - _Shield) / 100f);
						Hero.instance.Motor._anima.SetTrigger("TakeHitWhenBlocking");
				}
				else
				{
						CameraShaker.Instance.ShakeOnce(4f, 10f, .1f, .5f);
						Hero.instance.audioManager.Play(Hero.AudioClips.Hit.ToString());
						_Health -= damage;
						Hero.instance.Motor._anima.SetTrigger("TakeHit");
				}
		}

		public void ResetHeroDealAttack()
		{
		}
		/*
		private void OnGUI()
		{
						string boxText =
								"Level = " + _Level + "\n" +
								"Attack = " + _attack + "\n" +
								"Agility = " + _Agility + "\n";
						GUI.Box(new Rect(0, 0, 150, 100), boxText);
		}
		*/
		void AddAgility()
		{
				if (_Agility == 100)
						return;

				_SpeedAttack += _SpeedAttack * 0.002f;
				_DeltaRoll -= 0.1f;
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
