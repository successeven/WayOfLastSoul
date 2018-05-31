using System.Collections;
using System;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    protected HeroManager _heroManager;
    protected HeroMotor _heroMotor;
    public float _attack;
    public float _HP;

    bool _DealDamage = false;


    Animator _anima;
    bool _death = false;
		EnemyController _controller;

    private void Start()
    {
        _heroMotor = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroMotor>();
        _heroManager = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroManager>();
        _anima = GetComponent<Animator>();
				_controller = GetComponent<EnemyController>();

		//		SetStartSkills();
    }

    private void Update()
    {
        if (_HP <= 0 && !_death)
        {
            _death = true;
            _anima.SetTrigger("Death");
      //      GetComponent<Unit>().Die();
        }
    }

    protected virtual void SetStartSkills()
    {
        throw new NotImplementedException();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
				if (collision.tag == "Player" && _controller._attacks)
        {
            if (!_DealDamage)
            {
                _DealDamage = true;
								Hero.instance.TakeDamage(_attack);
            }
        }
    }

    public void ResetEnemyDealAttack()
    {
        _DealDamage = false;
    }

}
