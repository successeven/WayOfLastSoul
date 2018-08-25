using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlag : MonoBehaviour {

    bool _FireOn = false;
    public GameObject EffectObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !_FireOn)
        {
            EffectObject.SetActive(true);
            _FireOn = true;
        }
    }
}
