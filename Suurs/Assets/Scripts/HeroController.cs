using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour {

    [SerializeField]
    private float speed = 3.0F;
    //[SerializeField]
    //private float jumpForce = 15.0F;

    private bool isGrounded = false;


    new Rigidbody2D _rigidbody;
   // SpriteRenderer _sprite;
    Animator _anima;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
      //  _sprite = GetComponentInChildren<SpriteRenderer>();
        _anima = GetComponent<Animator>();
    }


	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Horizontal"))
            Run(Input.GetAxis("Horizontal"));
    }

    private void Run(float inValue)
    {
        _anima.SetFloat("Speed", inValue);
        Debug.Log(inValue);

        Vector3 direction = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);

       // _sprite.flipX = direction.x < 0.0F;

      //  if (isGrounded) State = CharState.Run;
    }
}
