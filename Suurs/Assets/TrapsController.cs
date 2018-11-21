using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapsController : MonoBehaviour {


		void OnTriggerEnter2D(Collider2D collision)
		{
				Debug.Log("TRAPS! ");
				if (collision.tag == "Player")
				{
						Hero.instance.Manager.DeathSpikes();
				}
		}
}
