using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    [Header("Cкорость развора камеры")]
    [SerializeField]
    int deltaCameraSpeed = 13;

    Vector3 _offset;

    [Space(10)]
    [SerializeField]
    public float _X = 10;

    [SerializeField]
    public float _Y = 1;

    float _CurrentX;
    float _horBound;//расстояние до границы камеры по горизонтали
    float _leftBound, _rightBound;        //левая, правая граница соответственно
    Camera _camera;

    GameObject _startPos;
    GameObject _finishPos;
    GameObject _Player;
    float _MoveRightSide = 1; //1- вправо ; -1 влево
    // Use this for initialization
    void Start()
    {
        _camera = GetComponent<Camera>();
        _horBound = _camera.orthographicSize / Screen.height * Screen.width;
        _CurrentX = _X;
    }

    void LateUpdate()
    {
        if (_startPos == null && _finishPos == null)
        {
            _startPos = GameObject.FindGameObjectWithTag("Start");
            _finishPos = GameObject.FindGameObjectWithTag("Finish");
            _Player = GameObject.FindGameObjectWithTag("Player");
        }

        if (_MoveRightSide != _Player.transform.localScale.x)
        {
            _X *= -1;
            _MoveRightSide = _Player.transform.localScale.x;
        }

        if (_CurrentX > _X)
            _CurrentX -= Time.deltaTime * deltaCameraSpeed;
        else if(_CurrentX < _X)
            _CurrentX += Time.deltaTime * deltaCameraSpeed;

        if (Mathf.Abs(_CurrentX - _X) <= 0.3f)
            _CurrentX = _X;

        _offset = new Vector3(_CurrentX, _Y, -20);
        Vector3 position = _Player.transform.position + _offset;
        _leftBound = position.x - _horBound;
        _rightBound = position.x + _horBound;
        if (_startPos != null)
            if (_startPos.transform.position.x >= _leftBound)
                position.x = _startPos.transform.position.x + _horBound;

        if (_finishPos != null)
            if (_finishPos.transform.position.x <= _rightBound)
                position.x = _finishPos.transform.position.x - _horBound;

        transform.position = position;
    }
}
