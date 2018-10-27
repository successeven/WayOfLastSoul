using UnityEngine;

public class FlipControll : MonoBehaviour
{

		[SerializeField]
		bool _rotatePosX = false;

		// Update is called once per frame
		void Update()
		{
				var scale = transform.localScale;
				if (_rotatePosX && scale.x != transform.root.transform.localScale.x)
				{
						var position = transform.position;
						Debug.Log(transform.root.transform.localScale.x);
						position.x *= transform.root.transform.localScale.x;
						transform.position = position;
				}
				scale.x = transform.root.transform.localScale.x;
				transform.localScale = scale;
						

		}
}
