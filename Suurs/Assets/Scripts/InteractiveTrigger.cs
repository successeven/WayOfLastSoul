using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveTrigger : MonoBehaviour {
     [Header("При активации запускает ломание")]
    public bool breakItemInicialise;
    [Header("Шанс дропа (из 100)")]
    [Range(0,100)]
    public byte dropChance = 25;
    public GameObject brokeEffect;
    public GameObject[] dropItems = new GameObject[1];
    [Header ("Номер dropItems, который должен падать.")]
    [Tooltip("Помни, что отсчет начинается от нуля")]
    public int dropGameObjectMumber = 0;
    bool active;


    void Update()
    {
        if (active)
        {
            if (breakItemInicialise)
            {
                GameObject g = Instantiate(brokeEffect);
                g.transform.position = transform.position;
                g.transform.localScale = transform.localScale;
                int chance = Random.Range(1, 100);
                if (chance < dropChance)
                    SpawnHealingComponent();

                Destroy(g, 2);
                Destroy(gameObject);

            }
        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            active = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            active = false;
        }
    }

    void SpawnHealingComponent()
    {
        if (dropItems[dropGameObjectMumber] != null)
        {
            GameObject hs = Instantiate(dropItems[dropGameObjectMumber]);
            hs.transform.position = transform.position + Vector3.up/2;
            if (hs.GetComponent<Rigidbody2D>() != null)
                hs.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        }
    }
}
