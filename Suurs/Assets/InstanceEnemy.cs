using UnityEngine;

public class InstanceEnemy : MonoBehaviour
{
		[SerializeField]
		GameObject _prefabEnemy;

		private void OnTriggerStay2D(Collider2D collision)
		{
				if (collision.tag == "Player")
				{
						Instantiate(_prefabEnemy, transform.position, Quaternion.identity);
						Destroy(gameObject);
				}
		}
}
