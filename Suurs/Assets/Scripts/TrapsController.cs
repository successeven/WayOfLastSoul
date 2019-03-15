using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapsController : MonoBehaviour {


		void OnTriggerEnter2D(Collider2D collision)
		{
				if (collision.tag == "Player")
				{ 
					Vector3 wayToHeroNormolize = Vector3.Normalize(transform.position - Hero.instance.transform.position); 
					Vector3 wayToUpNormolize = Vector3.Normalize(Vector3.up); 
				
					Debug.Log ("wayToHeroNormolize - " + wayToHeroNormolize );
					Debug.Log ("wayToUpNormolize - " + wayToUpNormolize) ;
					Debug.Log ("DOT - " + Vector3.Dot(wayToHeroNormolize, wayToUpNormolize) );
						Hero.instance.Manager.DeathSpikes();
				}
		}
}
