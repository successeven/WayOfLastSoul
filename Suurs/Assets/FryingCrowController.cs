using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryingCrowController : MonoBehaviour {

    bool _FlyOn = false;
    public GameObject EffectObject;
    public GameObject SpriteObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !_FlyOn)
        {
            SpriteObject.SetActive(false);
            EffectObject.SetActive(true);
            _FlyOn = true;
        }
    }
}
