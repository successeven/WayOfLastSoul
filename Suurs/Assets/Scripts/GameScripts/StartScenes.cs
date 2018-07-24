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
				Finish = 1,
				Exit = 2
		}

		public GameObject _StartImage;
		public Image _SceneName;
		[SerializeField]
		float _LenghtNameScene;

		GameObject _StartPos;
		GameObject _FinishPos;
		Image _imageSprite;
		Image _imageName;
		float _startDistance; //Дистанция до начала.

		bool _isLoaded = false;
		bool _showUIController = false;
		StateScene _stateScene = StateScene.Load;
		float percent;
		int alpha;
		// Use this for initialization
		void Start()
		{
				_imageSprite = _StartImage.GetComponent<Image>();
				_StartPos = GameObject.FindGameObjectWithTag("Start");
				_FinishPos = GameObject.FindGameObjectWithTag("Finish");
				_startDistance = (int)Mathf.Abs((Hero.instance.transform.position.x - _StartPos.transform.position.x));
		}

		private void LateUpdate()
		{
				float distance;
				switch (_stateScene)
				{
						case StateScene.Load:
								distance = (int)Mathf.Abs((Hero.instance.transform.position.x - _StartPos.transform.position.x));
								if (distance == 0)
								{
										Hero.instance.Controller._interfaceBlocked = false;
										_stateScene = StateScene.Game;
										UIController.instance.ShowUI();
								}
								else
								{
										var deltaSpeed = distance > 5 ? 0.9f : distance > 3 ? 0.7f : distance > 1 ? 0.3f : 0.11f;
										Hero.instance.Move(deltaSpeed * Hero.instance.transform.localScale.x);
								}

								if (distance > _LenghtNameScene)
								{
										percent = (_LenghtNameScene - _startDistance - distance) / _startDistance * 100f;
										alpha = (int)Mathf.Round((255 * (percent * 0.01f)));
										if (alpha > 255)
												alpha = 255;
										_SceneName.color = new Color32(0, 0, 0, (byte)(255 - alpha));
								}
								else
								{
										_SceneName.color = new Color32(0, 0, 0, 0);
										percent = (_startDistance - distance) / _startDistance * 100f;
										alpha = (int)Mathf.Round((255 * (percent * 0.01f)));
										if (alpha > 255)
												alpha = 255;
										_imageSprite.color = new Color32(0, 0, 0, (byte)(255 - alpha));
								}

								break;
						case StateScene.Game:
								bool changeLocation = false;
								if (Hero.instance.transform.position.x <= _StartPos.transform.position.x - 5)
								{
										PlayerPrefs.SetInt("NextLVL", 4); //назад всегда возвращаемся на респ
										changeLocation = true;
								}

								if (Hero.instance.transform.position.x >= _FinishPos.transform.position.x + 5)
								{
										int CompletedLVL = PlayerPrefs.GetInt("CompletedLVL");
										if (CompletedLVL < 3) //временная проверка конца игры...
												CompletedLVL++;

										if (SceneManager.GetActiveScene().name == "Respawn")
												PlayerPrefs.SetInt("NextLVL", CompletedLVL);
										else
												PlayerPrefs.SetInt("CompletedLVL", CompletedLVL);

										changeLocation = true;
								}

								if (changeLocation)
								{
										_stateScene = StateScene.Finish;
										UIController.instance.HideUI();
										StartCoroutine(AsyncLoad());
								}
								break;
						case StateScene.Finish:
								var closePos = _StartPos.transform.position.x - Hero.instance.transform.position.x > 0 ? _StartPos : _FinishPos;
								distance = (int)Mathf.Abs((Hero.instance.transform.position.x - closePos.transform.position.x));
								Hero.instance.Controller._interfaceBlocked = true;
								Hero.instance.Move(.9f * Hero.instance.transform.localScale.x);
								percent = 100f - (30f - distance) / 30f * 100f;
								alpha = (int)Mathf.Round((255 * (percent * 0.01f)));
								if (alpha > 255)
								{
										_isLoaded = true;
										_stateScene = StateScene.Exit;
								}
								else
										_imageSprite.color = new Color32(0, 0, 0, (byte)alpha);
								break;

						case StateScene.Exit:
								//ничего не делаем т к пошла загрузка... 
								break;
				}

		}

		IEnumerator AsyncLoad()
		{
				yield return new WaitForSeconds(1);
				AsyncOperation operation = SceneManager.LoadSceneAsync("Loading");
				operation.allowSceneActivation = false;

				while (!operation.isDone)
				{
						if (operation.progress == 0.9f)
						{
								if (UIController.instance.ad.IsLoaded())
										UIController.instance.ad.Show();
								if (_isLoaded)
										operation.allowSceneActivation = true;
						}
						yield return null;
				}
		}
}
