using System.Collections;
using UnityEngine;

public class TrapPlatform : MonoBehaviour
{

		Rigidbody2D _rigidbody;
		public float DeathTime = 2f;
		public GameObject ParticlePrefab;
		public float _shakingTimer;
		public float ShakingStrength;
		public bool FallOutFromColliders;
		 AudioSource Sound;
		public GameObject _platform;

		bool shaking = false;
		Vector3 _startPos, _newPos;
		
		void Start()
		{
				_rigidbody = GetComponent<Rigidbody2D>();
				Sound = GetComponent<AudioSource>();
				_startPos = _platform.transform.position;
		}

		// Update is called once per frame
		void FixedUpdate()
		{
				if (shaking)
				{
						float y = Random.Range(_startPos.y - ShakingStrength, _startPos.y + ShakingStrength);
						_newPos = _startPos;
						_newPos.y = y;
						_platform.transform.position = _newPos;
				}
		}

		void OnCollisionEnter2D(Collision2D col)
		{
				Debug.Log(col.collider.tag);
				if (col.collider.tag == "Player")
				{
						StartCoroutine(Falling());
						if (!shaking)
						{
								ParticlePrefab.SetActive(true);
								// if (!Sound.isPlaying)
								// 		Sound.Play();
						}
						shaking = true;
				}
		}

		IEnumerator Falling()
		{
				yield return new WaitForSeconds(_shakingTimer);
				ParticlePrefab.SetActive(false);
				_rigidbody.bodyType = RigidbodyType2D.Dynamic;
				_rigidbody.gravityScale = 5;

				if (FallOutFromColliders)
						GetComponent<Collider>().isTrigger = true;
				shaking = false;
				Destroy(gameObject, DeathTime);
				yield return 0;
		}
}
