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
    bool _death = false;

    bool _deathSpikes = false;


    [NonSerialized]
    public bool _TakeDamage = false;

    private void Awake()
    {
        LoadData();
    }

    private void FixedUpdate()
    {
        if (_Health <= 0 && !_death)
        {
            Hero.instance.audioManager.Play(Hero.AudioClips.Death.ToString());
            _death = true;



            if (_deathSpikes)
                Hero.instance.Motor._anima.SetTrigger("Death_Spikes");
            else
                Hero.instance.Motor._anima.SetTrigger("Death");
            GameOver();

            //Invoke("GameOver", 3f);
        }
    }

    public void GameOver()
    {
        UIController.instance.ResetHero();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && Hero.instance.Motor._attacks)
        {
            AliveObject enemyManager = collision.transform.gameObject.GetComponent<AliveObject>();
            if (enemyManager == null)
            {
                Debug.LogWarning("Не найден менеджер врага");
                return;
            }

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
            Hero.instance.Motor.CurrentHorAxis = 0;
            CameraShake.instance.Shake();
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
        Hero.instance.Motor.Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
    }

    void LoadData()
    {

    }


    public void ResetHero()
    {
        _death = false;
        _Health = _MaxHealth;
        Hero.instance.Motor._anima.SetTrigger("Reset");
        UIController.instance.ShowUI();
        Hero.instance.Motor.Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
