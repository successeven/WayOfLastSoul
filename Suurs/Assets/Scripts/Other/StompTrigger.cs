using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompTrigger : MonoBehaviour {

    public GameObject fallingObject;
    bool needMore = true;
    GameObject fall;


    void OnTriggerEnter2D(Collider2D col)
    {
       
        if (col.tag == "Player")
        {
          
            if (fall == null)
                needMore = true;
            else
                needMore = false;

            if (needMore)
            fall = Instantiate(fallingObject, GetComponentInChildren<Transform>());

        }
    }
}
