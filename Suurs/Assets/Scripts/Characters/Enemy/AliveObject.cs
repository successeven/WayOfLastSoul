using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliveObject : MonoBehaviour {

		public float _HP;

		public virtual void TakeHit(float damage, int attackID)
		{
		}

		protected virtual void Death() { }

		protected virtual void Die()
		{
				Destroy(transform.gameObject);
		}
}
