using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRenderController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var allObjects = GameObject.FindObjectsOfType<SpriteRenderer>();
        foreach (var item in allObjects)
        {
            item.gameObject.AddComponent<SpriteRenderSwicher>();

        }
	}
	
}
