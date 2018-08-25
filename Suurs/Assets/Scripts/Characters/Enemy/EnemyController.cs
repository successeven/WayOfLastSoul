using System.Collections;
using System;
using UnityEngine;

public class EnemyController : Unit
{

    [SerializeField]
    protected float _visibility = 25;


    [SerializeField]
    protected int _deltaTimeAttack = 2000;

    [SerializeField]
    protected float _deltaDistanceAttack = 2f;
    [SerializeField]
    protected float _deltaDistanceSpeed = 2f;

    protected Animator _anima;
    protected EnemyManager _enemyManager;
    protected Rigidbody2D _rigidbody;
    protected AudioManager audioManager;

    public bool _canAttack = true;
    public bool _canMove = true;

    protected bool _moving = false;
    protected bool _reciveDamage = false;
    [NonSerialized]
    public bool _attacks = false;
    protected float _lastAttackTime;
    protected float _distance;

    protected float _moveLeftSide = -1; //1- вправо ; -1 влево

    void Start()
    {
        _anima = GetComponent<Animator>();
        _enemyManager = GetComponent<EnemyManager>();
        _rigidbody = transform.root.GetComponent<Rigidbody2D>();
        audioManager = GetComponent<AudioManager>();
        AfterStart();
    }

    protected virtual void AfterStart()
    {
    }

    void Update()
    {

        if (_enemyManager._death)
            return;

        if (_enemyManager._HP <= 0 && !_enemyManager._death)
        {
            _enemyManager._death = true;
            _anima.SetTrigger("Death");
            Invoke("Die", 3f);
        }

        if (Vector3.Distance(Hero.instance.transform.position, transform.position) > 50)
            _anima.enabled = false;
        else
            _anima.enabled = true;


        if (Hero.instance.Manager._Health <= 0)
        {
            _anima.SetBool("Move", false);
            return;
        }
    }

    protected virtual void FixedUpdate()
    {
        _distance = Vector2.Distance(transform.position, Hero.instance.transform.position);
        if (_enemyManager._HP <= 0 || Hero.instance.Manager._Health <= 0)
            return;

        if (_canMove)
            DoMotion();

        if (_canAttack)
            DoAttack();
    }


    protected virtual void DoMotion()
    {
    }

    public virtual void TakeHit(float damage)
    {
    }

    protected virtual void DoAttack()
    {
    }

    protected virtual bool CanMove()
    {
        return true;
    }

    protected virtual void Die()
    {
        Destroy(transform.root.gameObject);
    }
}
