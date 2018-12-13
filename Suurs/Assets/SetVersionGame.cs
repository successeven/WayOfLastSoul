using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetVersionGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
				GetComponent<Text>().text = "Alpha v" + Application.version;		
	}
	
}
