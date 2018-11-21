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

		//public int _Level = 1; ///уровень
		public float _MaxHealth = 100; ///Максимальная жизнь	
		public float _Health = 100; ///Текущая жизнь
		public float _attack = 25; ///атака

		[Range(0, 100)]
		public float _Shield = 50f; ///Щит   

		public List<AttackItem> _attackItems;
		Animator _anima;
		bool _death = false;

		bool _deathSpikes = false;
        

		[NonSerialized]
		public bool _TakeDamage = false;

		private void Awake()
		{
				LoadData();
				_anima = GetComponent<Animator>();
		}

		private void FixedUpdate()
		{
				if (_Health <= 0 && !_death)
				{
            Hero.instance.audioManager.Play(Hero.AudioClips.Death.ToString());
            _death = true;
            if (_deathSpikes)
                _anima.SetTrigger("Death_Spikes");
            else
                _anima.SetTrigger("Death");
						GameOver();

						//Invoke("GameOver", 3f);
				}
		}

		public void GameOver()
		{
				UIController.instance.GameOver();
		}
		
		void OnTriggerEnter2D(Collider2D collision)
		{
				if (collision.tag == "Enemy" && Hero.instance.Motor._attacks)
				{
                    EnemyManager enemyManager = collision.transform.gameObject.GetComponent<EnemyManager>();
                    if (enemyManager == null)
                            return;

                    var _currentAttackItem = _attackItems.Where(x => x._ID == Hero.instance.Motor.AttackIndex).FirstOrDefault();
                    enemyManager.TakeHit(_currentAttackItem._damage, _currentAttackItem._ID);
				}
		}
		
		public void TakeDamage(float damage) //Урон
		{
				_TakeDamage = true;
				if (Hero.instance.Motor._blocking)
				{
						Hero.instance.audioManager.Play(Hero.AudioClips.Block.ToString());
						_Health -= damage * ((100f - _Shield) / 100f);
						Hero.instance.Motor._anima.SetTrigger("TakeHitWhenBlocking");
				}
				else
				{
						Hero.instance.Motor.FinishAllAttacks();
						CameraShaker.Instance.ShakeOnce(4f, 10f, .1f, .5f);
						Hero.instance.audioManager.Play(Hero.AudioClips.Hit.ToString());
						_Health -= damage;
						if (Hero.instance.Motor.CanBreakAnim())
						  Hero.instance.Motor._anima.SetTrigger("TakeHit");
				}
		}

		public void DeathSpikes() 
		{
            _TakeDamage = true;
            Hero.instance.Motor.FinishAllAttacks();
            _Health = 0;
            _deathSpikes = true;
		}

		void LoadData()
		{

		}
}
