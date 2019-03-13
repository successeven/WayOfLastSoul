using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEHealing : MonoBehaviour {

    [Header("Сколько будет длиться лечение")]
    public float timeOfHealing = 5;

    [Header("Время между волнами лечения")]
    [Tooltip("0 - постоянное лечение, больше, например 3: раз в 3 секунды дает +х здоровья")]
    public float timeBetweenHealWaves = 0;
    [Header("Количество восстанавливаемого здоровья")]
    public float healthHealing = 1;
    bool activateHealing;
    bool playerInZone;
    GameObject player;
    float tb = 0;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        if (activateHealing)
        {
            timeOfHealing -= Time.deltaTime;

            if (playerInZone)
            {
                if (timeBetweenHealWaves != 0)
                {
                    tb -= Time.deltaTime;
                    if (tb <= 0)
                    {
                        tb = timeBetweenHealWaves;
                        Debug.Log("Зона лечения осстановила здоровье на " + healthHealing);
                    }
                }
                else
                {
                    Debug.Log("Игрок получает лечение от зоны здоровья в размере " + healthHealing);
                }
            }

            if (timeOfHealing <= 0)
                Destroy(gameObject);

        }
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            activateHealing = true;
            playerInZone = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            playerInZone = false;
        }
    }
}
