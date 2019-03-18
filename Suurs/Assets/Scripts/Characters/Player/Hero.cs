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
        Walk,
        Hit,
        Uppercut,
        Strike_1,
        Strike_2,
        Strike_3,
        Shield_Attack,
        Dodge,
        Block,
        Death,
        Jump,
        Respawn //супергеройское приземление
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

    private void Start()
    {
        audioManager = GetComponent<AudioManager>();
        _heroController._checkPointPosition = transform.position;
    }

    public void Move(float inSpeed)
    {
        _heroMotor.CurrentHorAxis = inSpeed;
        _heroMotor.Move();
    }

    public void ResetHero()
    {
        _heroManager.ResetHero();
        _heroController.ResetHeroPosition();

    }

    public void SetCheckPointPosition(Vector3 position)
    {
        _heroController._checkPointPosition = position;
        _heroController._checkPointPosition.y += 6;
    }
}
