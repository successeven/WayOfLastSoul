using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingComponent : MonoBehaviour
{

    
    public float restoreHP;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            Debug.Log("Игроку восстановили ХП в размере " + restoreHP);
            Destroy(gameObject);
        }
    }
}