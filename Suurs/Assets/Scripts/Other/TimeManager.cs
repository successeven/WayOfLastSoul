using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    public float _mainTime = 0;
    float _currentTime = 0;

	// Use this for initialization
	void Start () {
        _mainTime = 0;
       /* if (PlayerPrefs.HasKey("GameTime"))
            _mainTime = PlayerPrefs.GetFloat("GameTime");
        else
            PlayerPrefs.SetFloat("GameTime", _mainTime);*/
        _currentTime = _mainTime;
    }
	
	void Update ()
    {
        _currentTime += Time.deltaTime;
        if (_mainTime - _currentTime >= 1)
        {
            _mainTime += _currentTime;
            PlayerPrefs.SetFloat("GameTime", _mainTime);
        }
    }
}
