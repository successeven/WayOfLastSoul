using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapsController : MonoBehaviour
{

    float DOT;
    Vector3 wayToHeroNormolize;
    Vector3 wayToUpNormolize;
    public float _minDot, _maxDot;


    private void Update() {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            wayToHeroNormolize = transform.position - Hero.instance.Motor.GroundChecker.position;
            wayToUpNormolize = Vector3.Normalize(Vector3.up);
            DOT = Vector3.Dot(Vector3.Normalize(wayToHeroNormolize), wayToUpNormolize);
            if (DOT > _minDot && DOT < _maxDot)
                Hero.instance.Manager.DeathSpikes();
        }
    }

}
