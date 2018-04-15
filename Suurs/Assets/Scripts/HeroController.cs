using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : Unit {

    [SerializeField]
    private float speed = 3.0F;
    
    new Rigidbody2D _rigidbody;
    Animator _anima;

    private bool _acingRight = true;
    private bool _jumping = false;
    private bool _attaks = false;



    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _anima = GetComponent<Animator>();
    }
    

    private void FixedUpdate()
    {
        Debug.Log(_anima.GetFloat("Attack"));
        if (!_attaks && !_jumping)
        {
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            Run(h);
        }
        if (CrossPlatformInputManager.GetButtonDown("Fire1") && !_jumping)
            Attack(1);

        if (CrossPlatformInputManager.GetButtonDown("Jump") && !_attaks)
            Jump();

    }

    private void Jump()
    {
        _jumping = true;
        _anima.SetBool("Test", true);
        _anima.SetBool("Test", false);
        //_anima.SetTrigger("Jump");
    }

    private void Attack(float inTypeAttack)
    {
        _attaks = true;
        _anima.SetBool("Test", true);
        _anima.SetBool("Test", false);
        Debug.Log("Attack");
    }

    public void ResetAttack()
    {
        _attaks = false;
        Debug.Log("ResetAttack");
    }

    public void ResetJump()
    {
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
