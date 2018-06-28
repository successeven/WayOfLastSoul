using System.Collections;
using System;
using UnityEngine;

public class FallenController : EnemyController
{

    public void ResetEnemyAttack()
    {
        if (_attacks)
        {
            _attacks = false;
            _enemyManager.ResetEnemyDealAttack();
        }
    }

    void ResetTakeDamage()
    {
        _reciveDamage = false;
    }

    protected override void DoMotion()
    {
        _distance = Vector2.Distance(transform.position, Hero.instance.transform.position);
        if (_distance <= _visibility && _distance > _deltaDistanceAttack && !_attacks && !_reciveDamage)
        {
            _anima.SetBool("Move", true);
						var deltaSpeed = (_distance > _deltaDistanceSpeed + 2 + _deltaDistanceAttack ? 0.7f :
														  _distance > _deltaDistanceSpeed + _deltaDistanceAttack ? 0.5f : 0.3f);
						
						if (transform.root.position.x - Hero.instance.transform.position.x > 0)
                _moveLeftSide = -deltaSpeed;
            else
                _moveLeftSide = deltaSpeed;


						bool actionRight = false;
            if (transform.root.localScale.x < 0)
                actionRight = true;

            Move(_rigidbody, _speed, ref actionRight, _moveLeftSide);
        }
        else
            _anima.SetBool("Move", false);
    }


    protected override void DoAttack()
    {
        int currentDeltaAttack = (int)Math.Truncate((Time.fixedTime - _lastAttackTime) * 1000);
        if ((_distance <= _deltaDistanceAttack) && !_attacks && currentDeltaAttack > _deltaTimeAttack && !_reciveDamage)
        {
            _attacks = true;
            _anima.SetTrigger("Attack");
            _lastAttackTime = Time.fixedTime;
        }
    }

    public override void TakeHit()
    {
        _anima.SetTrigger("TakeHit");
		}

}
