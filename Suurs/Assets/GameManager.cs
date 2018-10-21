using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

#if UNITY_ANDROID
				if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
				{
						if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
						{

						}
				}
#endif
		}
}
