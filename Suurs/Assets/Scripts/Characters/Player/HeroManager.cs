using EZCameraShake;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class HeroData
{
		public int _Level = 1; ///уровень
		public float _MaxHealth = 100; ///Максимальная жизнь	
		public float _Health = 100; ///Текущая жизнь

		public float _attack = 25; ///атака

		[Range(0, 100)]
		public float _Shield = 50f; ///Щит   
}

public class HeroManager : MonoBehaviour
{
		public HeroData _data;

		public int _Level = 1; ///уровень
		public float _MaxHealth = 100; ///Максимальная жизнь	
		public float _Health = 100; ///Текущая жизнь
		public float _attack = 25; ///атака

		[Range(0, 100)]
		public float _Shield = 50f; ///Щит   

		public List<AttackItem> _attackItems;
		Animator _anima;
		bool _death = false;

		[NonSerialized]
		public bool _TakeDamage = false;

		private void Awake()
		{
				LoadData();
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

		void LoadData()
		{

		}
}
