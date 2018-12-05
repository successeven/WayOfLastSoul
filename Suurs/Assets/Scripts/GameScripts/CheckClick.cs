using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

public class CheckClick : MonoBehaviour
{

		RaycastHit2D hit2D;
		Vector3 _touchPos;
		Vector2 _RayPos;

		RaycastHit hit;
		Ray ray;

		// Update is called once per frame
		void Update()
		{
				bool wasClick = false;

#if UNITY_ANDROID
				if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
						if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
						{
								_touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
								wasClick = true;
						}
#endif

#if UNITY_EDITOR
				if (Input.GetMouseButtonDown(1))
				{
						_touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
						wasClick = true;
				}
#endif

				if (wasClick)
				{
						_touchPos.z = 100;
						hit2D = Physics2D.Raycast(_touchPos, Camera.main.transform.position, Mathf.Infinity);
						Debug.DrawLine(_touchPos, Camera.main.transform.position, Color.red, Mathf.Infinity);
						if (hit2D)
						{
								Debug.Log(hit2D.transform.tag);
								if (hit2D.transform.CompareTag("Finish"))
										GameManager.instance.PlayFromTimelines(0);
						}
				}
		}

}
