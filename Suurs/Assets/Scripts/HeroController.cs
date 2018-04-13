using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : Unit {

    [SerializeField]
    private float speed = 3.0F;
    
    new Rigidbody2D _rigidbody;
    Animator _anima;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _anima = GetComponent<Animator>();
    }


    // Update is called once per frame

    private void FixedUpdate()
    {
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
            Run(h);
    }

    private void Run(float inH)
    {
        Vector3 direction = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
        Debug.Log(Input.GetAxis("Horizontal"));
        _anima.SetFloat("Speed", Mathf.Abs(inH));
    }

}
