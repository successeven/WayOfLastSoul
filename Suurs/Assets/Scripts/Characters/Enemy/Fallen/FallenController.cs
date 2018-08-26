using System.Collections;
using System;
using UnityEngine;

public class FallenController : EnemyController
{
    enum FallenSounds
    {
        None,
        Move,
        Strike,
        Death,
        When_Hit
    }

    [SerializeField]
    float _damageLength = 2f;


    public void ResetEnemyAttack()
    {
        if (_attacks)
        {
            _attacks = false;
            _enemyManager.ResetEnemyDealAttack();
        }
    }

    protected override bool CanMove()
    {
        return (!_attacks && !_reciveDamage && _canMove);
    }

    protected override void DoMotion()
    {
        bool actionRight = false;
        if (transform.localScale.x < 0)
            actionRight = true;
        if (_distance <= _visibility && CanMove())
        {
            _moveLeftSide = 0;
            if (_distance >= _deltaDistanceAttack)
            {
                _anima.SetBool("Move", true);
                if (!audioManager.IsPlaying(FallenSounds.Move.ToString()))
                    audioManager.Play(FallenSounds.Move.ToString());
                var deltaSpeed = (_distance > _deltaDistanceSpeed + 2 + _deltaDistanceAttack ? 0.7f :
                                                    _distance > _deltaDistanceSpeed + _deltaDistanceAttack ? 0.5f : 0.3f);

                if (transform.position.x - Hero.instance.transform.position.x > 0)
                    _moveLeftSide = -deltaSpeed;
                else
                    _moveLeftSide = deltaSpeed;

                Move(_rigidbody, _speed, _moveLeftSide);
            }
            else
            {
                _anima.SetBool("Move", false);
                audioManager.Stop(FallenSounds.Move.ToString());
            }

            if (Hero.instance.transform.position.x - transform.position.x > 0 && !actionRight)
                Flip(ref actionRight);
            else if (Hero.instance.transform.position.x - transform.position.x < 0 && actionRight)
                Flip(ref actionRight);
        }
    }


    protected override void DoAttack()
    {
        int currentDeltaAttack = (int)Math.Truncate((Time.fixedTime - _lastAttackTime) * 1000);
        if ((_distance <= _deltaDistanceAttack) && !_attacks && currentDeltaAttack > _deltaTimeAttack && !_reciveDamage)
        {
            audioManager.Play(FallenSounds.Strike.ToString());
            _attacks = true;
            _anima.SetTrigger("Attack");
            _lastAttackTime = Time.fixedTime;
        }
    }

    public override void TakeHit(float damage)
    {
        _reciveDamage = true;
        audioManager.Play(FallenSounds.When_Hit.ToString());
        _rigidbody.velocity = new Vector2(_damageLength * transform.localScale.x, _rigidbody.velocity.y);
        _enemyManager._HP -= damage;
        if (_enemyManager._HP <= 0)
            return;
        _anima.SetTrigger("TakeHit");
        _reciveDamage = false;
    }

    protected override void Die()
    {
        audioManager.Play(FallenSounds.Death.ToString());
        base.Die();
    }

}
