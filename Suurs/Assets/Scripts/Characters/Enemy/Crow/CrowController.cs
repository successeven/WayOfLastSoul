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
    bool _startAttack = false;
    StateCrow stateCrow = StateCrow.Idle;
    int _moveSide = 1; //1- вправо ; -1 влево
		bool _animTakeOff = true;

		protected override void DoMotion()
    {
        if (Hero.instance.Manager._Health <= 0 && stateCrow != StateCrow.Idle)
            stateCrow = StateCrow.TakeOff;

				switch (stateCrow)
        {
            case StateCrow.Idle:
                _distance = Math.Abs(transform.position.x - Hero.instance.transform.position.x);
                if (_distance < _visibility)
                    stateCrow = StateCrow.Move;
                break;
            case StateCrow.Move:
                #region MOVE
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
                    Mathf.Sin(Time.realtimeSinceStartup * _verticalSpeed) * _amplitude + Hero.instance.transform.position.y + 8
                    );

                transform.position = _tempPosition;

                int currentDeltaAttack = (int)Math.Truncate((Time.fixedTime - _lastAttackTime) * 1000);
                if (currentDeltaAttack > _deltaTimeAttack && !_reciveDamage)
                {
                    _distance = Vector2.Distance(transform.position, Hero.instance.transform.position);
                    if (_distance >= _distanceAttack - _deltaDistanceAttack && _distance <= _distanceAttack + _deltaDistanceAttack)
                    {
                        _startAttack = true;

                        actionRight = transform.root.localScale.x < 0;
                        if (_moveSide > 0 && transform.position.x - Hero.instance.transform.position.x > 0)
                            Flip(ref actionRight);
                        else if (_moveSide < 0 && transform.position.x - Hero.instance.transform.position.x < 0)
                            Flip(ref actionRight);
                        _anima.SetTrigger("Fly");
                        _startAttackPosition = transform.position;
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

                _distance = Vector3.Distance(transform.position, Hero.instance.transform.position);
                if (_distance <= 2f && !_attacks)
                {
                    _attacks = true;
										_anima.SetTrigger("Attack");//, _attacks);
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
								_attacks = false;
								if (_animTakeOff)
								{
										_anima.SetTrigger("TakeOff");
										_animTakeOff = false;
								}
								transform.position = Vector2.MoveTowards(transform.position, _startAttackPosition, _SpeedAttack * Time.deltaTime);
                if (transform.position == _startAttackPosition)
                {
                    stateCrow = StateCrow.Idle;
                    _startAttack = false;
                    _enemyManager.ResetEnemyDealAttack();
                }
                break;
        }
    }

    public override void TakeHit()
    {
        _anima.SetBool("Attack", false);
        _attacks = false;
        _anima.SetTrigger("TakeHit");
        stateCrow = StateCrow.TakeOff;
    }
}
