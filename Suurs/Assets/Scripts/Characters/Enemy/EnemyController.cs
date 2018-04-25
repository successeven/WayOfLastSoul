using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Unit {


    [SerializeField]
    float _visibility = 25;

    [SerializeField]
    float _speed = 3f;

    [SerializeField]
    int _speedAttack = 2000;

    Animator _Anima;
    bool _Moving = false;
    bool _Attack = false;
    bool _TakeDamage = false;
    float _lastAttackTime;

    GameObject _Player;
    HeroManager _PlayerManager;
    HeroController _PlayerController;
    FallenManager _FallenManager;

    float _distance;
    Rigidbody2D _rigidbody;

    float _MoveRightSide = -1; //1- вправо ; -1 влево

    void Start () {
        _Player = GameObject.FindGameObjectWithTag("Player");
        _PlayerManager = _Player.GetComponent<HeroManager>();
        _PlayerController = _Player.GetComponent<HeroController>();
        _Anima = GetComponent<Animator>();
        _rigidbody = transform.root.GetComponent<Rigidbody2D>();
        _FallenManager = GetComponent<FallenManager>();
    }
	
	void FixedUpdate () {

        if (_FallenManager._HP <= 0)
            return;


        _distance = Vector2.Distance(transform.position, _Player.transform.position);
        int playerHP = _Player.GetComponent<HeroManager>()._HP;

        if (playerHP <= 0)
        {
            _Anima.SetBool("Move", false);
            return;
        }

        if (_distance <= _visibility && _distance > 2f && !_Attack && !_TakeDamage)
        {
            _Anima.SetBool("Move", true);
            bool actionRight = transform.root.position.x - _Player.transform.position.x < 0;

            if (_MoveRightSide != transform.root.localScale.x)
                _MoveRightSide = transform.root.localScale.x ;

            Move(_rigidbody, _speed, ref actionRight, -_MoveRightSide);
        }
        else
        {
            _Anima.SetBool("Move", false);
        }
        int deltaAttack = (int)Math.Truncate((Time.fixedTime - _lastAttackTime) * 1000);
        if ((_distance <= 2f) && !_Attack && deltaAttack > _speedAttack && !_TakeDamage)
        {
            _Attack = true;
            _Anima.SetTrigger("Attack");
            _lastAttackTime = Time.fixedTime;
        }        
 
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerSword")
        {
            if (!_TakeDamage)
            {
                _TakeDamage = true;
                _FallenManager._HP -= _PlayerManager._Attack;

                if (_FallenManager._HP <= 0)
                {
                    _Anima.SetTrigger("Death");
                    Invoke("Die", 3f);
                }
                else
                    _Anima.SetTrigger("TakeHit");
            }
        }
    }


    void ResetAttack()
    {
        _Attack = false;
    }

    void ResetTakeDamage()
    {
        Debug.Log("ResetTakeDamage");
        _TakeDamage = false;
    }
}
