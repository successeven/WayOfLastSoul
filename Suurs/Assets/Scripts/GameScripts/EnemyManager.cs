using System.Collections;
using System;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    protected HeroManager _heroManager;
    protected HeroController _heroController;
    public int _attack;
    public int _HP;

    bool _DealDamage = false;


    Animator _anima;
    bool _death = false;
		EnemyController _controller;

    private void Start()
    {
        _heroController = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroController>();
        _heroManager = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroManager>();
        _anima = GetComponent<Animator>();
				_controller = GetComponent<EnemyController>();

				SetStartSkills();
    }

    private void Update()
    {
        if (_HP <= 0 && !_death)
        {
            _death = true;
            _anima.SetTrigger("Death");
            GetComponent<Unit>().Die();
        }
    }

    protected virtual void SetStartSkills()
    {
        throw new NotImplementedException();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.transform.root.name);
				if (collision.tag == "Player" && _controller._attacks)
        {
            if (!_DealDamage)
            {
                _DealDamage = true;

                if (_heroController._jumping)
                    return;
                else if (_heroController._blocking)
                    _heroManager._HP -= (int)Math.Truncate(_attack * (_heroManager._Shield / 100));
                else
                    _heroManager._HP -= _attack;

                if (_heroController._blocking)
                    _heroController._anima.SetTrigger("TakeHitWhenBlocking");
                else
                    _heroController._anima.SetTrigger("TakeHit");

            }
        }
    }

    public void ResetEnemyDealAttack()
    {
        _DealDamage = false;
    }

}
