using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
		[Header("Cкорость развора камеры")]
		[SerializeField]
		float deltaCameraSpeed = 13f;

		[Space(10)]
		[Header("Смещение камеры")]
		public Vector3 _offset;

		float _MoveRightSide = 1; //1- вправо ; -1 влево
		
		void FixedUpdate()
		{
				if (Hero.instance == null)
						return;
        Debug.Log("HERE");
				if (_MoveRightSide != Hero.instance.transform.localScale.x)
				{
						_offset.x *= -1;
						_MoveRightSide = Hero.instance.transform.localScale.x;
				}
				Vector3 desiredPosition = Hero.instance.transform.position + _offset;
				transform.root.transform.position = Vector3.Lerp(transform.root.transform.position, desiredPosition, deltaCameraSpeed);

		}
}
