using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class StartScenes : MonoBehaviour {

    public GameObject _UIController;
    public GameObject _StartPos;
    public GameObject _StartImage;
    HeroController _heroController;
    GameObject _Player;
    float _startDistance;
	// Use this for initialization
	void Start () {
        _Player = GameObject.FindGameObjectWithTag("Player");
        _heroController = _Player.GetComponent<HeroController>();
        _startDistance = (int)Mathf.Abs((_Player.transform.position.x - _StartPos.transform.position.x));
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        float distance = (int)Mathf.Abs((_Player.transform.position.x - _StartPos.transform.position.x));

        if (distance == 0)
        {
            _UIController.SetActive(true);
            _heroController._interfaceBlocked = false;
            enabled = false;
        }
        else
            _heroController.Move(distance > 3 ? 0.9f : distance > 2 ? 0.6f : distance > 1 ? 0.3f: 0.1f);

        float percent = (_startDistance - distance) / _startDistance * 100f;
        Image imageSprite = _StartImage.GetComponent<Image>();
        int alpha = (int)Mathf.Round((255 * (percent * 0.01f)));
        if (alpha > 255)
            alpha = 255;
        imageSprite.color = new Color32(0, 0, 0, (byte)(255 - alpha));
    }
}
