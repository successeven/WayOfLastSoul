using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainbusterManager : MonoBehaviour {

    [SerializeField]
    float _attack = 10f;

    void OnTriggerEnter2D (Collider2D collision) 
    {
        if (collision.tag == "Player") 
            Hero.instance.Manager.TakeDamage (_attack);
    }
}
