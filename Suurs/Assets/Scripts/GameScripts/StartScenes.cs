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
        PreLoad = -2,
        Load = -1,
        Game = 0,
        Finish = 1,
        Exit = 2
    }
    
    public GameObject _StartImage;
    public Image _SceneName;

    GameObject _StartPos;
    GameObject _FinishPos;
    Image _imageSprite;
    Image _imageName;
    AudioManager audioManager;
    float _startDistance; //Дистанция до начала.

    bool _isLoaded = false;
    bool _showUIController = false;
    StateScene _stateScene = StateScene.PreLoad;
    float percent;
    int alpha;
    // Use this for initialization
    void Start()
    {
        audioManager = GetComponent<AudioManager>();
        _imageSprite = _StartImage.GetComponent<Image>();
        _StartPos = GameObject.FindGameObjectWithTag("Start");
        _FinishPos = GameObject.FindGameObjectWithTag("Finish");
        _startDistance = (int)Mathf.Abs((Hero.instance.transform.position.x - _StartPos.transform.position.x));
    }

    void ChangeState()
    {
        _stateScene = StateScene.Load;
    }

    private void LateUpdate()
    {
        float distance;
        switch (_stateScene)
        {
            case StateScene.PreLoad:
                // Нужно ли тут что то?! 
                break;
            case StateScene.Load:
                if (!audioManager.IsPlaying("Background"))
                    audioManager.Play("Background");

                distance = (int)Mathf.Abs((Hero.instance.transform.position.x - _StartPos.transform.position.x));
                if (distance == 0)
                {
                    _imageSprite.enabled = false;
                    Hero.instance.Controller._interfaceBlocked = false;
                    _stateScene = StateScene.Game;
                    UIController.instance.ShowUI();
                }
                else
                {
                    var deltaSpeed = distance > 5 ? 0.9f : 0.7f;
                    Hero.instance.Move(deltaSpeed * Hero.instance.transform.localScale.x);
                }

                _SceneName.color = new Color32(0, 0, 0, 0);
                percent = (_startDistance - distance) / _startDistance * 100f;
                alpha = (int)Mathf.Round((255 * (percent * 0.01f)));
                if (alpha > 255)
                    alpha = 255;
                _imageSprite.color = new Color32(0, 0, 0, (byte)(255 - alpha));

                break;
            case StateScene.Game:
                bool changeLocation = false;
                if (Hero.instance.transform.position.x <= _StartPos.transform.position.x - 5)
                {
                    PlayerPrefs.SetInt("NextLVL", -4); //назад всегда возвращаемся на респ
                    if (Hero.instance.transform.localScale.x > 0)
                        Hero.instance.Motor.Flip();
                    changeLocation = true;
                }

                if (Hero.instance.transform.position.x >= _FinishPos.transform.position.x + 5)
                {

                    int CompletedLVL = PlayerPrefs.GetInt("CompletedLVL");
                    if (CompletedLVL >= 1) //временная проверка конца игры...
                        CompletedLVL++;
                    else
                        CompletedLVL = 1;

                    if (SceneManager.GetActiveScene().name == "Respawn")
                        PlayerPrefs.SetInt("NextLVL", CompletedLVL);
                    else
                    {
                        PlayerPrefs.SetInt("CompletedLVL", CompletedLVL);
                        PlayerPrefs.SetInt("NextLVL", CompletedLVL + 1);
                    }

                    changeLocation = true;
                }

                if (changeLocation)
                {
                    _stateScene = StateScene.Finish;
                    _imageSprite.enabled = true;
										var count = PlayerPrefs.GetInt("CountChangeLevel");
										count++;
										PlayerPrefs.SetInt("CountChangeLevel", count);
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
                if (UIController.instance.ad.IsLoaded() && PlayerPrefs.GetInt("CountChangeLevel") == 3)
								{
										UIController.instance.ad.Show();
										PlayerPrefs.SetInt("CountChangeLevel", 0);
								}
                if (_isLoaded)
                    operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
