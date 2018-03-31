using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour {

    public Transform _lookAt;

    Vector3 _offset = new Vector3(2f , 0.5f, -1f);
    /*
    public float a = 7f;
    public float b = 4.5f;
    public float c = -1f;
    */

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
       // _offset = new Vector3(a, b, c);
        transform.position = _lookAt.transform.position + _offset;
		
	}
}
