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
    public const string resetUIAD = "ca-app-pub-4537576181628162/9500630169";
    

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
    public InterstitialAd reset_ad;

    void Start()
    {
        _anima = GetComponent<Animator>();
        MobileAds.Initialize(appId);
        ad = new InterstitialAd(GameOverAD);
        AdRequest request = new AdRequest.Builder().Build();
        ad.LoadAd(request);
        
        reset_ad = new InterstitialAd(resetUIAD);
        AdRequest requestReset = new AdRequest.Builder().Build();
        reset_ad.LoadAd(requestReset);

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
        Invoke("ExitGame", 6f);
    }

    void NextLevel()
    {
        if (ad.IsLoaded() && gameOver)
            ad.Show();
        PlayerPrefs.SetInt("NextLVL", 0);
        SceneManager.LoadScene("Loading");
    }

    public void ContinueGame()
    {
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
        
        var resetTimes = PlayerPrefs.GetInt("ResetTimes");
        if (resetTimes > 3)
          GameOver();
        else
        {
            PlayerPrefs.SetInt("ResetTimes", resetTimes++);
            _state = StateScene.ResetHero;
            needChange = true;
        }
    }

    public void btn_ResetHero_NO_Click()
    {
        GameOver();
    }
    
    public void btn_ResetHero_YES_Click()
    {
        if (reset_ad.IsLoaded())
            reset_ad.Show();
        GameManager.instance.PlayFromTimelines(GameTimeLines.ResetUI);
        
        AdRequest requestReset = new AdRequest.Builder().Build();
        reset_ad.LoadAd(requestReset);
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
