using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRenderSwicher : MonoBehaviour {

    SpriteRenderer spriteRenderer;
	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();		
	}
	
	// Update is called once per frame
	void Update () {
        if(spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();		

        var distance = Vector3.Distance(transform.position, Hero.instance.transform.position);        
        if (distance > 100) 
           spriteRenderer.enabled = false;
        else
           spriteRenderer.enabled = true;
		
	}
}
