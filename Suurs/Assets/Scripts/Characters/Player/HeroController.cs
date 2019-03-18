using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(HeroManager))]
[RequireComponent(typeof(HeroMotor))]
public class HeroController : MonoBehaviour
{
    bool _doubleBlock = false;
    float _lastBlockClickTime = 0;
    bool _holdBlock = false;
    float _catchTime = .3f;

    [NonSerialized]
    public bool _holdAttack = false;
    [NonSerialized]
    public float _holdAttackTime;


    public Vector3 _checkPointPosition;

    public bool _interfaceBlocked = true;


    float _currentHorAxis = 0;

    float _startShieldAttackTime;
    bool _shieldAttackFull = false;
    void FixedUpdate()
    {

        if (Hero.instance.Manager._Health <= 0)
            return;

        if (_interfaceBlocked)
            return;

        _currentHorAxis = CrossPlatformInputManager.GetAxis("Horizontal");

        int k = 1;
        if (_currentHorAxis < 0)
            k = -1;

        if (Math.Abs(_currentHorAxis) >= 0.2 && Math.Abs(_currentHorAxis) < 0.8)
            Hero.instance.Motor.CurrentHorAxis = 0.4f * k;
        else if (Math.Abs(_currentHorAxis) >= 0.8)
            Hero.instance.Motor.CurrentHorAxis = _currentHorAxis;
        else
            Hero.instance.Motor.CurrentHorAxis = 0;


    }

    float _timeAttackHold;
    float _timeLastJump;
    float _deltaLastTimeJump = 0.5f;
    private void Update()
    {


        if (Hero.instance.Manager._Health <= 0)
            return;

        if (_interfaceBlocked)
            return;

        if (CrossPlatformInputManager.GetButtonDown("Attack") && !Hero.instance.Motor._blocking)
        {
            _holdAttack = !Hero.instance.Motor._attacks;
            if (!Hero.instance.Motor._attacks)
                _holdAttackTime = -1;
            else
                _holdAttackTime = Time.fixedTime;

            _shieldAttackFull = false;
            _holdAttackTime = Time.fixedTime;
        }

        if (_holdAttack)
        {
            if (Time.fixedTime - _holdAttackTime > Hero.instance.Motor._shield_AttackTime)
                _shieldAttackFull = true;
        }

        if (_holdAttack && _holdAttackTime != -1 && Time.fixedTime - _holdAttackTime > 0.2f)
        {
            float deltaPower = (Time.fixedTime - _holdAttackTime);
            Hero.instance.Motor._anima.SetFloat("Shield Power", deltaPower);
        }
        else
            Hero.instance.Motor._anima.SetFloat("Shield Power", 0);


        if (CrossPlatformInputManager.GetButtonUp("Attack"))
        {
            _holdAttack = false;
            _holdAttackTime = -1;
            if (_shieldAttackFull)
            {
                _shieldAttackFull = false;
                Hero.instance.Motor.ShieldAttack();
            }
            else
                Hero.instance.Motor.Attack();
        }

        if (CrossPlatformInputManager.GetButtonDown("Jump") && Hero.instance.Motor.m_Grounded
                && Time.fixedTime - _timeLastJump > _deltaLastTimeJump)
        {
            _timeLastJump = Time.fixedTime;
            Hero.instance.Motor.Jump();

        }

        if (CrossPlatformInputManager.GetButtonDown("Uppercut"))
            Hero.instance.Motor.Uppercut();

        float timeDelta = Time.time - _lastBlockClickTime;
        if (CrossPlatformInputManager.GetButtonDown("Block"))
        {
            _holdBlock = true;
            _lastBlockClickTime = Time.fixedTime;
            if (timeDelta < _catchTime)
            {
                _doubleBlock = true;
                _holdBlock = false;
            }
        }

        if (_holdBlock && timeDelta >= _catchTime)
        {
            Hero.instance.Motor.SetBlock();
            _holdBlock = false;
        }

        if (CrossPlatformInputManager.GetButtonUp("Block"))
            Hero.instance.Motor.UnSetBlock();


        if (_doubleBlock)
        {
            float deltaBackSlide = Time.fixedTime - Hero.instance.Motor._lastDodgeTime;
            Hero.instance.Motor.Dodge();
            _doubleBlock = false;
        }
    }

    //Возвращаем героя на последний CheckPoint
    public void ResetHeroPosition()
    {
        transform.position = _checkPointPosition;
        Hero.instance.Motor.ResetScale();
    }

    public void ResetStats()
    {
        Hero.instance.Manager._TakeDamage = false;
    }
}
