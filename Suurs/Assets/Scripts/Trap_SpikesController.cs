using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_SpikesController : MonoBehaviour {

    public float _attack = 10f;
    bool _DealAttack = false;

	// Use this for initialization
	void Start () {
        Invoke("Die", 5f);
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
           if (!_DealAttack)
           {
               Hero.instance.Manager.TakeDamage(_attack);
               _DealAttack = true;
           }           
        }     
    }

    void Die()
    {
        Destroy(transform.gameObject);
    }
}
