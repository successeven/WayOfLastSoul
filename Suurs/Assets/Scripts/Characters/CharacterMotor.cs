﻿using System.Collections;
using System;
using UnityEngine;

public class CharacterMotor : MonoBehaviour
{
    [SerializeField]
    float _speed = 16f;

    protected Rigidbody2D _rigidbody;

    [NonSerialized]
    public Animator _anima;


    protected bool _acingRight = true;

    // Use this for initialization
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _anima = GetComponent<Animator>();
    }


    protected virtual bool CanMove()
    {
        return true;
    }

    public virtual void Move(float inMoveDirection)
    {
        _anima.SetFloat("Speed", Mathf.Abs(inMoveDirection));
        if (CanMove())
        {
            if (!Hero.instance.Manager._TakeDamage)
                if (inMoveDirection != 0 && !AudioManager.instance.IsPlaing(Hero.AudioClips.Run.ToString()))
                    AudioManager.instance.Play(Hero.AudioClips.Run.ToString());
                else if (inMoveDirection == 0)
                    AudioManager.instance.Stop(Hero.AudioClips.Run.ToString());

            AudioManager.instance.SetPitch(Hero.AudioClips.Run.ToString(), Math.Abs(inMoveDirection));
            _rigidbody.velocity = new Vector2(inMoveDirection * _speed, _rigidbody.velocity.y);
            if (inMoveDirection > 0 && !_acingRight)
                Flip();
            else if (inMoveDirection < 0 && _acingRight)
                Flip();
        }
    }

    protected void Flip()
    {
        // Switch the way the player is labelled as facing.
        _acingRight = !_acingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.root.localScale;
        theScale.x *= -1;
        transform.root.localScale = theScale;

    }
}
