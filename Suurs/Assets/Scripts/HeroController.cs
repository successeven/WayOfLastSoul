using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : Unit {

    [SerializeField]
     float speed = 3.0F;

    [SerializeField]
     float rollLength = 3.0F;

    Rigidbody2D _rigidbody;
    Animator _anima;

    private bool _acingRight = true;
    private bool _jumping = false;
    private bool _attacks = false;



    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _anima = GetComponent<Animator>();
    }
    

    private void FixedUpdate()
    {
        Debug.Log(_attacks + " " + _jumping);
        if (!_attacks && !_jumping)
        {
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            Run(h);
        }
        if (CrossPlatformInputManager.GetButtonDown("Fire1") && !_jumping)
            Attack(1);

        if (CrossPlatformInputManager.GetButtonDown("Jump") && !_attacks && !_jumping)
            Jump();

    }

    private void Jump()
    {
        _jumping = true;
        _anima.SetFloat("Speed", 0);
        _anima.SetTrigger("Jump");
        _rigidbody.velocity = new Vector2(rollLength * transform.localScale.x, -1);
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

    public void ResetJumping()
    {
        Debug.Log("ResetJump");
        _jumping = false;
    }


    private void Run(float inH)
    {
        Vector3 direction = transform.right * inH;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
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
