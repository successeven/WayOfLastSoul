using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour {

    [SerializeField]
    private float speed = 3.0F;
    //[SerializeField]
    //private float jumpForce = 15.0F;

    private bool isGrounded = false;


    new Rigidbody2D rigidbody;
    SpriteRenderer sprite;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }


	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Horizontal")) Run();
    }

    private void Run()
    {
        Vector3 direction = transform.right * Input.GetAxis("Horizontal");

        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);

        sprite.flipX = direction.x < 0.0F;

      //  if (isGrounded) State = CharState.Run;
    }
}
