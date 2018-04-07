using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{

    [SerializeField]
    Transform _lookAt;

    Vector3 _offset;

    public float _a;
    public float _b;
    float _horBound;//расстояние до границы камеры по горизонтали
    float _leftBound, _rightBound;        //левая, правая граница соответственно
    Camera _camera;

    GameObject _startPos;
    GameObject _finishPos;
    // Use this for initialization
    void Start()
    {
        _camera = GetComponent<Camera>();
        _horBound = _camera.orthographicSize / Screen.height * Screen.width;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (_startPos == null && _finishPos == null)
        {
            _startPos = GameObject.FindGameObjectWithTag("Start");
            _finishPos = GameObject.FindGameObjectWithTag("Finish");
        }
        _offset = new Vector3(_a, _b, -20);

        Vector3 position = _lookAt.transform.position + _offset;
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
