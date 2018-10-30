using UnityEngine;

public class CameraPosition : MonoBehaviour
{

		[SerializeField]
		GameObject _camera;


		// Update is called once per frame
		void Update()
		{
				transform.position = _camera.transform.position;
		}
}
