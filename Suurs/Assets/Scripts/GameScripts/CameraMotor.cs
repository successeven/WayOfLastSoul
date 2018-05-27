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

    GameObject _Player;
    float _MoveRightSide = 1; //1- вправо ; -1 влево

    void Start()
    {
        _Player = GameObject.FindGameObjectWithTag("Player");
    }


	void FixedUpdate()
    {
        if (_MoveRightSide != _Player.transform.localScale.x)
        {
			_offset.x *= -1;
            _MoveRightSide = _Player.transform.localScale.x;
        }
		Vector3 desiredPosition = _Player.transform.position + _offset;
		transform.position = Vector3.Lerp(transform.position, desiredPosition, deltaCameraSpeed);

		//transform.LookAt(_Player.transform);
    }
}
