using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTrap : MonoBehaviour {

    public float _resetTime = 3f;
    public GameObject _TrapPrefab;
    float _timeDrop;

    bool _canDrop = true;
	
	// Update is called once per frame
	void Update () {
		if (!_canDrop && Time.fixedTime - _timeDrop > _resetTime)
          _canDrop = true;
	}

    void OnTriggerStay2D(Collider2D other)
    {        
        if (other.tag == "Player")
        {
           if (_canDrop)
           {
               Instantiate(_TrapPrefab, transform.position, Quaternion.identity);
               _timeDrop = Time.fixedTime;
               _canDrop = false;
               return;
           }   
        }     
    }
    
}
