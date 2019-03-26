using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointCode : MonoBehaviour
{

    public GameObject _CheckPointPrefab;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Instantiate(_CheckPointPrefab, transform.position, Quaternion.identity);
            Hero.instance.SetCheckPointPosition(transform.position);
            Destroy(gameObject);
            return;
        }
    }
}
