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
        {
            if (Time.time - _lastClickTime < _catchTime)
                _doubleAttack = true;
            else
                _doubleAttack = false;
            _lastClickTime = Time.time;
        }
    }

    private void FixedUpdate()
    {
        if (!_attacks && !_jumping && !_blocking)
        {
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            Run(h);
        }
        if (CrossPlatformInputManager.GetButton("Fire1") && !_jumping)
            if (_doubleAttack)
                Attack(2);
            else
                Attack(1);
        int deltaJump = (int)Math.Truncate((Time.fixedTime - _lastJumpTime) * 1000);
        if (CrossPlatformInputManager.GetButtonDown("Jump") && !_attacks && (deltaJump > _manager._DeltaRoll))
            Jump();

        if (CrossPlatformInputManager.GetButtonDown("Block") && !_blocking)
            SetBlock(); 

        if (CrossPlatformInputManager.GetButtonUp("Block") && _blocking)
            UnSetBlock();
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


    private void Run(float inH)
    {
        _rigidbody.velocity = new Vector2(inH * _speed, _rigidbody.velocity.y);
        //Vector3 direction = transform.right * inH;
        //transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
        _anima.SetFloat("Speed", Mathf.Abs(inH));
        if (inH > 0 && !_acingRight)
            Flip();
        else if (inH < 0 && _acingRight)
            Flip();
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        _acingRight = !_acingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

}
