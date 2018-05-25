using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
		[Header("Cкорость развора камеры")]
		[SerializeField]
		float deltaCameraSpeed = 13f;

		Vector3 _offset;

		[Space(10)]
		[SerializeField]
		public float _X = 10;

		[SerializeField]
		public float _Y = 1;

		GameObject _Player;
		float _MoveRightSide = 1; //1- вправо ; -1 влево

		void Start()
		{
				_Player = GameObject.FindGameObjectWithTag("Player");
		}

		void LateUpdate()
		{
				if (_MoveRightSide != _Player.transform.localScale.x)
				{
						_X *= -1;
						_MoveRightSide = _Player.transform.localScale.x;
				}

				_offset = new Vector3(_Player.transform.position.x + _X, _Player.transform.position.y + _Y, -20);
				transform.position = Vector3.MoveTowards(transform.position, _offset, deltaCameraSpeed * Time.deltaTime);
		}
}
