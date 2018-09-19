using System.Collections;
using UnityEngine;

public class TrapPlatform : MonoBehaviour
{

		Rigidbody rig;
		public float DeathTime = 2f;
		public GameObject ParticlePrefab;
		AudioSource Sound;
		public float fallingTimer;
		public float ShakingStrength;
		public bool FallOutFromColliders;
		bool shaking = false;
		float position;

		Vector3 trans;
		// Use this for initialization
		void Start()
		{
				rig = GetComponent<Rigidbody>();
				Sound = GetComponent<AudioSource>();
				position = transform.position.y;
				trans = transform.position;
		}

		// Update is called once per frame
		void Update()
		{
				if (shaking)
				{
						float y = Random.Range(position - ShakingStrength, position + ShakingStrength);
						transform.position = new Vector3(trans.x, y, trans.z);
				}
		}

		void OnCollisionEnter(Collision col)
		{
				if (col.collider.tag == "Player")
				{
						StartCoroutine(Falling());
						if (!shaking)
						{
								ParticlePrefab.SetActive(true);
								if (!Sound.isPlaying)
										Sound.Play();
						}
						shaking = true;
				}
		}


		IEnumerator Falling()
		{
				yield return new WaitForSeconds(fallingTimer);
				ParticlePrefab.SetActive(false);
				rig.isKinematic = false;

				if (FallOutFromColliders)
						GetComponent<Collider>().isTrigger = true;
				shaking = false;
				Destroy(gameObject, DeathTime);
				yield return 0;
		}
}
