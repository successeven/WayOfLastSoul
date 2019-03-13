using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour {

    public float activateTriggerTime = .6f;
    public float attackForce;
    byte x = 0;
	// Update is called once per frame
	void Update () {
        if (GetComponent<ParticleSystem>().time > activateTriggerTime)
        {
            GetComponent<Collider2D>().enabled = true;
        }
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        
            if (col.tag == "Player" && x ==0)
            {
                Debug.Log("Попадание атакой с нанесением урона " + attackForce);
            x++;
            }
    }
}
