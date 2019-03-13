using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDropObject : MonoBehaviour {

    public GameObject VFX;
    public Transform spawn;
    void Start()
    {
       // Destroy(gameObject, 5);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Floor")
        {
            GameObject ve = Instantiate(VFX);
            ve.transform.position = spawn.transform.position;

            Destroy(gameObject);
        }
    }
}
