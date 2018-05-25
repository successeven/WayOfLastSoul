﻿using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class StartScenes : MonoBehaviour {

    public GameObject _UIController;
    public GameObject _StartImage;
    GameObject _StartPos;
    GameObject _FinishPos;
    HeroController _heroController;
    GameObject _Player;
    Image _imageSprite;
    float _startDistance; //Дистанция до начала.

    bool _isLoaded = false;
    bool _showUIController = false;
    Animator _UIanimator;
    // Use this for initialization
    void Start () {
        _Player = GameObject.FindGameObjectWithTag("Player");
        _heroController = _Player.GetComponent<HeroController>();
        _imageSprite = _StartImage.GetComponent<Image>();
        _UIanimator = _UIController.GetComponent<Animator>();
        _StartPos = GameObject.FindGameObjectWithTag("Start");
        _FinishPos = GameObject.FindGameObjectWithTag("Finish");
        _startDistance = (int)Mathf.Abs((_Player.transform.position.x - _StartPos.transform.position.x));
    }
    

    // Update is called once per frame
    void Update () {
        if (!_isLoaded)
        {
            float distance = (int)Mathf.Abs((_Player.transform.position.x - _StartPos.transform.position.x));
            if (distance == 0)
            {
                _isLoaded = true;
                //_UIController.SetActive(true)
                _heroController._interfaceBlocked = false;
                _UIanimator.enabled = true;
            }
            else
                _heroController.Move(distance > 3 ? 0.9f : distance > 2 ? 0.8f : distance > 1 ? 0.7f : 0.4f, _StartPos);

            float percent = (_startDistance - distance) / _startDistance * 100f;
            int alpha = (int)Mathf.Round((255 * (percent * 0.01f)));
            if (alpha > 255)
                alpha = 255;
            _imageSprite.color = new Color32(0, 0, 0, (byte)(255 - alpha));
        }
        else
        {
            float distance = (int)Mathf.Abs((_Player.transform.position.x - _FinishPos.transform.position.x));
            if (distance > 30f || distance == 0)
                return;

            _heroController._interfaceBlocked = true;

            if (distance == 0)
                transform.root.gameObject.SetActive(false);

            _heroController.Move(distance > 3 ? 0.9f : distance > 2 ? 0.8f : distance > 1 ? 0.7f : 0.4f, _FinishPos);


            _UIanimator.SetBool("Show", false);
            float percent = (30f - distance) / 30f * 100f;
            int alpha = (int)Mathf.Round((255 * (percent * 0.01f)));
            if (alpha > 255)
                alpha = 255;
            _imageSprite.color = new Color32(0, 0, 0, (byte)alpha);

        }
    }
}
