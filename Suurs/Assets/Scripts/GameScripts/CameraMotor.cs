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

    float _CurrentX;

    GameObject _startPos;
    GameObject _finishPos;
    GameObject _Player;
    float _MoveRightSide = 1; //1- вправо ; -1 влево
    // Use this for initialization
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
				/*
				if (_CurrentX > _X)
						_CurrentX -= Time.deltaTime * deltaCameraSpeed;
				else if(_CurrentX < _X)
						_CurrentX += Time.deltaTime * deltaCameraSpeed;

				if (Mathf.Abs(_CurrentX - _X) <= 0.3f)
						_CurrentX = _X;

				_offset = new Vector3(_CurrentX, _Y, -20);
				Vector3 position = _Player.transform.position + _offset;

			 /* _leftBound = position.x - _horBound;
				_rightBound = position.x + _horBound;
				if (_startPos != null)
						if (_startPos.transform.position.x >= _leftBound)
								position.x = _startPos.transform.position.x + _horBound;

				if (_finishPos != null)
						if (_finishPos.transform.position.x <= _rightBound)
								position.x = _finishPos.transform.position.x - _horBound;*/

				_offset = new Vector3(_Player.transform.position.x + _X, _Player.transform.position.y + _Y, -20);
				transform.position = Vector3.Lerp(transform.position, _offset, deltaCameraSpeed *Time.deltaTime);
    }
}
