using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireOn : MonoBehaviour {

    bool _FireOn = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !_FireOn)
        {
            foreach(var item in GetComponentsInChildren<ParticleSystem>())
            {
                item.Play();
            }
            _FireOn = true;
        }          

    }
}
