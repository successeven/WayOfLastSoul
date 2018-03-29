using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

    // Use this for initialization
    void Start () {
        var temp = transform.Find("ConnectionPuzzle");
        if (temp != null)
            Debug.Log("ok");
        else
            Debug.Log("не ok");
        

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
