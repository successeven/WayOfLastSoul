using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CheckClick : MonoBehaviour {
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
						hit2D = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));

						//_touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
						//ray = Camera.main.ScreenPointToRay(Input.mousePosition);
						//_touchPos.z = 10f;
						wasClick = true;
				}
#endif


				if (wasClick)
				{
						//_RayPos = new Vector2(_touchPos.x, _touchPos.y);
						//hit2D = Physics2D.Raycast(_RayPos, _RayPos);
						if (hit2D)
						{
								Debug.Log(hit2D.transform.tag + " " + hit2D.transform.gameObject.name);
								if (hit2D.transform.CompareTag("Finish"))
										GameManager.instance.PlayFromTimelines(0);
						}
				}
		}

}
