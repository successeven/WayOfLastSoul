using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{

		[SerializeField]
		GameObject _effects;

		void OnTriggerEnter2D(Collider2D col)
		{
				_effects.SetActive(col.tag == "Wall");						
		}

		void OnTriggerExit2D(Collider2D col)
		{
				_effects.SetActive(!(col.tag == "Wall"));
		}

}
