using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour {

    public Transform _lookAt;

    Vector3 _offset = new Vector3(7f , 4.5f, -1f);



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
        transform.position = _lookAt.transform.position + _offset;
		
	}
}
