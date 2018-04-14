using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : Unit {

    [SerializeField]
    private float speed = 3.0F;
    
    new Rigidbody2D _rigidbody;
    Animator _anima;

    private bool m_FacingRight = true;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _anima = GetComponent<Animator>();
    }
    
    private void Update()
    {
        if (!m_Jump)
        {
            // Read the jump input in Update so button presses aren't missed.
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }
    }

    private void FixedUpdate()
    {
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        Run(h);
    }


    private void Run(float inH)
    {
        Vector3 direction = transform.right * inH;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
        Debug.Log(inH);
        _anima.SetFloat("Speed", Mathf.Abs(inH));
        if (inH > 0 && !m_FacingRight)
            Flip();
        else if (inH < 0 && m_FacingRight)
            Flip();
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

}
