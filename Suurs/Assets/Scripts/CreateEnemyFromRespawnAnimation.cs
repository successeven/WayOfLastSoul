using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEnemyFromRespawnAnimation : MonoBehaviour {

   public GameObject enemy;
    public float startTimer = 0.5f;
    // Use this for initialization

    void Update()
    {
        if (GetComponent<ParticleSystem>().time >startTimer)
        {
            GameObject e = Instantiate(enemy);
            e.transform.position = transform.position;
            Destroy(gameObject);
        }
    }
	
	
}
