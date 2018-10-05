using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(HeroMotor))]
[RequireComponent(typeof(HeroManager))]
[RequireComponent(typeof(HeroController))]
public class Hero : MonoBehaviour
{
    public enum AudioClips
    {
        None,
        Run,
        Roll,
        Hit,
        Strike_1,
        Strike_2,
        Strike_3,
        Rapira,
        Dodge,
        Block,
        Death
    }

    #region Singleton

    public static Hero instance;

    void Awake()
    {
        instance = this;
        _heroController = GetComponent<HeroController>();
        _heroManager = GetComponent<HeroManager>();
        _heroMotor = GetComponent<HeroMotor>();
    }

    #endregion

    HeroController _heroController;
    public HeroController Controller
    {
        get
        {
            return _heroController;
        }
    }

    HeroManager _heroManager;
    public HeroManager Manager
    {
        get
        {
            return _heroManager;
        }
    }


    HeroMotor _heroMotor;
    public HeroMotor Motor
    {
        get
        {
            return _heroMotor;
        }
    }
    public AudioManager audioManager { get; private set; }



    public const string GameOverAD = "ca-app-pub-4537576181628162/5928041347";
    public const string bannerAD = "ca-app-pub-4537576181628162/5626457313";

    private void Start()
    {
        audioManager = GetComponent<AudioManager>();
    }

    public void Move(float inSpeed)
    {
        _heroMotor.Move(inSpeed);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
