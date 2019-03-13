using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour {

    public GameObject spawnVFX;
    public GameObject spawnPoint;
    public GameObject spawnEnemyTrigger;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            GameObject g = Instantiate(spawnVFX);
            g.transform.position = spawnPoint.transform.position;
            if (spawnEnemyTrigger != null)
            {
                GameObject v = Instantiate(spawnEnemyTrigger);
                v.transform.position = spawnPoint.transform.position;
            }
            Destroy(g, 2);
            Destroy(spawnPoint);
            Destroy(gameObject);
        }
    }
}
