using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class StartScenes : MonoBehaviour
{
		enum StateScene
		{
				Load = -1,
				Game = 0,
				Finish = 1
		}

		public GameObject _UIController;
		public GameObject _StartImage;
		GameObject _StartPos;
		GameObject _FinishPos;
		Image _imageSprite;
		float _startDistance; //Дистанция до начала.

		bool _isLoaded = false;
		bool _showUIController = false;
		Animator _UIanimator;
		StateScene _stateScene = StateScene.Load;
		// Use this for initialization
		void Start()
		{
				_imageSprite = _StartImage.GetComponent<Image>();
				_UIanimator = _UIController.GetComponent<Animator>();
				_StartPos = GameObject.FindGameObjectWithTag("Start");
				_FinishPos = GameObject.FindGameObjectWithTag("Finish");
				_startDistance = (int)Mathf.Abs((Hero.instance.transform.position.x - _StartPos.transform.position.x));
		}


		// Update is called once per frame
		void Update()
		{ 
				if (!_isLoaded)
				{
						float distance = (int)Mathf.Abs((Hero.instance.transform.position.x - _StartPos.transform.position.x));
						if (distance == 0)
						{
								_isLoaded = true;
								Hero.instance.Controller._interfaceBlocked = false;
								_UIanimator.enabled = true;
						}
						else
								Hero.instance.Move(distance > 5 ? 0.9f : distance > 3 ? 0.7f : distance > 1 ? 0.3f : 0.11f);

						float percent = (_startDistance - distance) / _startDistance * 100f;
						int alpha = (int)Mathf.Round((255 * (percent * 0.01f)));
						if (alpha > 255)
								alpha = 255;
						_imageSprite.color = new Color32(0, 0, 0, (byte)(255 - alpha));
				}
				else
				{
						if (Hero.instance.transform.position.x >= _FinishPos.transform.position.x)
						{
								if (SceneManager.GetActiveScene().name == "Respawn")
										PlayerPrefs.SetInt("NextLVL", 3);
								else
										PlayerPrefs.SetInt("NextLVL", 2);

								SceneManager.LoadScene("Loading");
						}

						float distance = (int)Mathf.Abs((Hero.instance.transform.position.x - _FinishPos.transform.position.x));
						if (distance > 30f || distance == 0)
								return;

						Hero.instance.Controller._interfaceBlocked = true;

						if (distance == 0)
								transform.root.gameObject.SetActive(false);

						Hero.instance.Move(.9f);


						_UIanimator.SetBool("Show", false);
						float percent = (30f - distance) / 30f * 100f;
						int alpha = (int)Mathf.Round((255 * (percent * 0.01f)));
						if (alpha > 255)
								alpha = 255;
						_imageSprite.color = new Color32(0, 0, 0, (byte)alpha);
				}
		}
}
