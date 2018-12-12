using UnityEngine;

public class CameraPosition : MonoBehaviour
{

		[SerializeField]
		GameObject _camera;


		// Update is called once per frame
		void FixedUpdate()
		{
				transform.position = _camera.transform.position;
		}
}
