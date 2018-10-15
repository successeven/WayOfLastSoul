using System.Collections;
using System;
using UnityEngine;

public class CrowController : EnemyController
{

    enum StateCrow
    {
        Idle = 0,
        Move = 1,
        Fly = 2,
        Attack = 3,
        TakeOff = 4
    }

    enum SoundCrow
    {
        Move,
        Strike,
        Death,
        Voice,
        Voice_1,
        Voice_2
    }

    [SerializeField]
    float _SpeedAttack = 3f;

    [SerializeField]
    float _distanceAttack = 10;

    [Space(10)]
    [Header("Idle")]
    [SerializeField]
    float _verticalSpeed = 1;

    [SerializeField]
    float _amplitude = 2;

    [SerializeField]
    float _patrolLength;


    Vector2 _targetAttack;
    Vector2 _tempPosition;
    float _CurrentPosX;
    Vector3 _startAttackPosition;
    StateCrow stateCrow = StateCrow.Idle;
    int _moveSide = 1; //1- вправо ; -1 влево
    bool _animTakeOff = true;

    float _currentDeltaTimeAttack = 0;
    float lastVoicetime;
    float deltaTimeVoice = 0;
		int tempHeight;

		protected override void AfterStart()
		{
				tempHeight = UnityEngine.Random.Range(7, 11);
		}

		protected override void DoMotion()
    {
        if (Hero.instance.Manager._Health <= 0 && stateCrow != StateCrow.Idle)
            stateCrow = StateCrow.TakeOff;
        switch (stateCrow)
        {
            case StateCrow.Idle:
                if (_distance < _visibility)
                {
                    stateCrow = StateCrow.Move;
                    if (_currentDeltaTimeAttack == 0)
                    _currentDeltaTimeAttack = UnityEngine.Random.Range(2f, _deltaTimeAttack);
										//tempHeight = UnityEngine.Random.Range(7, 11);
								}
                else
                {
                    audioManager.StopAll();
                }
                break;
            case StateCrow.Move:
                #region MOVE

                if (!audioManager.IsPlaying(SoundCrow.Move.ToString()))
                    audioManager.Play(SoundCrow.Move.ToString());

                if (deltaTimeVoice == 0)
                {
                    lastVoicetime = Time.fixedTime;
                    deltaTimeVoice = UnityEngine.Random.Range(5f, 10f); //кричать с перерывом 5-10 сек 
                }

                if (Time.fixedTime - lastVoicetime > deltaTimeVoice)
                {
                    audioManager.Play(SoundCrow.Voice.ToString());
                    lastVoicetime = Time.fixedTime;
                    deltaTimeVoice = UnityEngine.Random.Range(5f, 10f); //кричать с перерывом 5-10 сек 
                }

                _CurrentPosX = transform.position.x;
                if (_CurrentPosX < Hero.instance.transform.position.x - _patrolLength)
                {
                    _moveSide = 1;
                }
                else if (_CurrentPosX > Hero.instance.transform.position.x + _patrolLength)
                {
                    _moveSide = -1;
                }

                _CurrentPosX += _speed * Time.deltaTime * _moveSide;

                bool actionRight = transform.root.localScale.x < 0;
                if (_moveSide > 0 && !actionRight)
                    Flip(ref actionRight);
                else if (_moveSide < 0 && actionRight)
                    Flip(ref actionRight);

                _tempPosition = new Vector3(
                    _CurrentPosX,
                    Mathf.Sin(Time.realtimeSinceStartup * _verticalSpeed) * _amplitude + Hero.instance.transform.position.y + tempHeight);

                transform.position = _tempPosition;

                var currentDeltaAttack = Time.fixedTime - _lastAttackTime;
                if (currentDeltaAttack > _currentDeltaTimeAttack && !_reciveDamage && _canAttack)
                {
                    if (_distance >= _distanceAttack - _deltaDistanceAttack && _distance <= _distanceAttack + _deltaDistanceAttack)
                    {

                        _currentDeltaTimeAttack = UnityEngine.Random.Range(2f, _deltaTimeAttack);
                        actionRight = transform.root.localScale.x < 0;
                        if (_moveSide > 0 && transform.position.x - Hero.instance.transform.position.x > 0)
                            Flip(ref actionRight);
                        else if (_moveSide < 0 && transform.position.x - Hero.instance.transform.position.x < 0)
                            Flip(ref actionRight);
                        _anima.SetTrigger("Dive");
												tempHeight = UnityEngine.Random.Range(7, 11);
												_tempPosition = new Vector3(
														_CurrentPosX,
														Mathf.Sin(Time.realtimeSinceStartup * _verticalSpeed) * _amplitude + Hero.instance.transform.position.y + tempHeight);

												_startAttackPosition = _tempPosition;

												_startAttackPosition.x -= 10 * transform.localScale.x;
                        _targetAttack = Hero.instance.transform.position;
                        
                        stateCrow = StateCrow.Fly;
                    }
                }
                break;
            #endregion 
            case StateCrow.Fly:
                _lastAttackTime = Time.fixedTime;
                transform.position = Vector2.MoveTowards(transform.position, _targetAttack, _SpeedAttack * Time.deltaTime);

                if (_distance <= 3f && !_attacks)
								{
										audioManager.Play(SoundCrow.Strike.ToString());
										_attacks = true;
                    _anima.SetTrigger("Attack");
                    stateCrow = StateCrow.Attack;
                    _animTakeOff = false;
                    break;
                }

                _distance = Vector2.Distance(transform.position, _targetAttack);
                if (_distance <= 0 && !_attacks)
                {
                    stateCrow = StateCrow.TakeOff;
                    _animTakeOff = true;
                }
                break;
            case StateCrow.Attack:
                transform.position = Vector2.MoveTowards(transform.position, _targetAttack, _SpeedAttack * Time.deltaTime);
                _distance = Vector2.Distance(transform.position, _targetAttack);
                if (_distance == 0)
                    stateCrow = StateCrow.TakeOff;
                break;
            case StateCrow.TakeOff:
                if (_animTakeOff)
                {
                    _anima.SetTrigger("TakeOff");
                    _animTakeOff = false;
								}
								transform.position = Vector2.MoveTowards(transform.position, _startAttackPosition, _SpeedAttack * Time.deltaTime);
                if (transform.position == _startAttackPosition)
                {
                    stateCrow = StateCrow.Idle;
                    _enemyManager.ResetEnemyDealAttack();
                }
                _attacks = false;
                break;
        }
    }

    public override void TakeHit(float damage)
    {
				_reciveDamage = true;
        _enemyManager._HP -= damage;
        if (_enemyManager._HP <= 0)
            return;
        _anima.SetBool("Attack", false);
        _attacks = false;
        _anima.SetTrigger("TakeHit");
        audioManager.Play(SoundCrow.Voice_2.ToString());
        stateCrow = StateCrow.TakeOff;
    }

    protected override void Die()
    {
        audioManager.StopAll();
        audioManager.Play(SoundCrow.Death.ToString());
        base.Die();
    }

		protected override void Death()
		{
				_rigidbody.gravityScale = 10;
		}
}
