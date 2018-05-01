using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HeroManager))]
public class HeroController : Unit
{

    [SerializeField]
    float _speed = 25F;

    [SerializeField]
    float _rollLength = 20F;


    Rigidbody2D _rigidbody;
    Animator _anima;

    int _comboAttack = 0;

    float _lastClickTime = 0;
    float _catchTime = .25f;
    bool _doubleAttack = false;
    bool _acingRight = true;
    bool _jumping = false;
    bool _takeHit = false;
    bool _attacks = false;
    bool _blocking = false;
    float _lastJumpTime = 0;

    HeroManager _manager;



    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _anima = GetComponent<Animator>();
        _manager = GetComponent<HeroManager>();
        _lastJumpTime = Time.fixedTime;
    }

    private void Update()
    {

        if (Input.GetButtonDown("Fire1"))
            if (_attacks)
                _comboAttack++;
        /*
            if (Time.time - _lastClickTime < _catchTime)
                _doubleAttack = true;
            else
                _doubleAttack = false;
            _lastClickTime = Time.time;
        }*/
    }

    private void FixedUpdate()
    {
        if (_manager._HP < 0)
            return;

        if (!_attacks && !_jumping && !_blocking)
        {
            float h = CrossPlatformInputManager.GetAxis("Horizontal");

            Move(_rigidbody, _speed,ref _acingRight, h);
            _anima.SetFloat("Speed", Mathf.Abs(h));
        }
        if (CrossPlatformInputManager.GetButton("Fire1") && !_jumping)
                Attack(_comboAttack++);

        int deltaJump = (int)Math.Truncate((Time.fixedTime - _lastJumpTime) * 1000);
        if (CrossPlatformInputManager.GetButtonDown("Jump") && !_attacks && (deltaJump > _manager._DeltaRoll))
            Jump();

        if (CrossPlatformInputManager.GetButtonDown("Block") && !_blocking)
            SetBlock();

        if (CrossPlatformInputManager.GetButtonUp("Block") && _blocking)
            UnSetBlock();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(12);

        if (collision.tag == "FallenSword")
        {
            Debug.Log("FallenSword");
            GameObject fallen = collision.transform.root.gameObject;

            FallenManager fallenManager = fallen.GetComponentInChildren<FallenManager>();
            if (!fallenManager._doAttack)
            {
                fallenManager._doAttack = true;
                if (_jumping)
                    return;
                if (_blocking)
                    _manager._HP -= (int)Math.Truncate(fallenManager._attack * (_manager._Shield / 100));
                else
                  _manager._HP -= fallenManager._attack;

                if (_manager._HP <= 0)
                    _anima.SetTrigger("Death");
                else if (_blocking)
                    _anima.SetTrigger("TakingHit");
                else
                    _anima.SetTrigger("TakeHit");

            }
        }
    }

    public void DisableAnima()
    {
        _anima.enabled = false;
    }

    private void SetBlock()
    {
        _blocking = true;
        _anima.SetTrigger("Block");
        _anima.SetBool("Blocking", _blocking);
    }

    private void UnSetBlock()
    {
        _blocking = false;
        _anima.SetBool("Blocking", _blocking);
    }
    private void Jump()
    {
        _jumping = true;
        _lastJumpTime = Time.fixedTime;
        _anima.SetFloat("Speed", 0);
        _anima.SetTrigger("Jump");
        _rigidbody.velocity = new Vector2(_rollLength * transform.localScale.x, 1);
    }

    private void Attack(float inTypeAttack)
    {
        _attacks = true;
        _anima.SetFloat("Speed", 0);
        _anima.SetFloat("Attack", inTypeAttack);
    }

    public void ResetAttack()
    {
        _anima.SetFloat("Attack", 0);
        _attacks = false;
    }


    public void ResetHit()
    {
        _takeHit = false;
    }

    public void ResetJumping()
    {
        _jumping = false;
    }



}
