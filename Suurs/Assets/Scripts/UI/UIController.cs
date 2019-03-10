using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;

public class UIController : MonoBehaviour
{

    #region Singleton

    public static UIController instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    public const string appId = "ca-app-pub-4537576181628162~8534023389";
    public const string GameOverAD = "ca-app-pub-4537576181628162/5928041347";
    public const string bannerAD = "ca-app-pub-4537576181628162/5626457313";

    enum StateScene
    {
        None = 0,
        Start = 1,
        Game = 2,
        Pause = 3,
        Exit = 4,
        ResetHero = 5
    }
    Animator _anima;


    bool needChange;

    bool gameLoad = false;
    StateScene _state = StateScene.None;
    public InterstitialAd ad;

    void Start()
    {
        _anima = GetComponent<Animator>();
        MobileAds.Initialize(appId);
        ad = new InterstitialAd(GameOverAD);
        AdRequest request = new AdRequest.Builder().Build();
        ad.LoadAd(request);
        gameOver = false;
    }

    public void ShowUI()
    {
        _state = StateScene.Start;
        needChange = true;
    }

    public void HideUI()
    {
        _state = StateScene.Exit;
        needChange = true;
    }

    public void PauseGame()
    {
        Debug.Log("PauseGame");
        _state = StateScene.Pause;
        needChange = true;
        Time.timeScale = 0.1f;
    }

    public void DisableTime()
    {
        Time.timeScale = 0;
    }

    public void NormalizeTime()
    {
        Time.timeScale = 1;

    }

    bool gameOver = false;
    public void GameOver()
    {
        gameOver = true;
        _anima.SetTrigger("GameOver");
        Invoke("NextLevel", 6f);
    }

    void NextLevel()
    {
        if (ad.IsLoaded() && gameOver)
            ad.Show();
        PlayerPrefs.SetInt("NextLVL", -1);
        SceneManager.LoadScene("Loading");
    }

    public void ContinueGame()
    {
        Debug.Log("Continue");
        _state = StateScene.Game;
        needChange = true;
        Time.timeScale = 0.1f;
    }

    public void ExitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    public void ResetHero()
    {
        _state = StateScene.ResetHero;
        needChange = true;
    }

    public void btn_ResetHero_NO_Click()
    {
        GameOver();
    }
    
    public void btn_ResetHero_YES_Click()
    {
        GameManager.instance.PlayFromTimelines(GameTimeLines.ResetUI);
        //Hero.instance.ResetHero();
    }


    // Update is called once per frame
    void Update()
    {
        if (!needChange)
            return;

        switch (_state)
        {
            case StateScene.Start:
                _anima.SetInteger("State", 1);
                break;

            case StateScene.Pause:
                _anima.SetInteger("State", 3);
                break;

            case StateScene.Game:
                _anima.SetInteger("State", 4);
                break;

            case StateScene.Exit:
                _anima.SetInteger("State", 2);
                break;

            case StateScene.ResetHero:
                _anima.SetInteger("State", 5);
                break;
        }
        

        _anima.SetTrigger("Change");
        needChange = false;
    }
}
