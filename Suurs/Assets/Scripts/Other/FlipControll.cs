using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipControll : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        var scale = transform.localScale;
        scale.x = transform.root.transform.localScale.x;
        transform.localScale = scale;

    }
}
