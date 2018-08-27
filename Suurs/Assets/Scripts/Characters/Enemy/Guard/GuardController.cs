using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardController : EnemyController
{

    enum GuardSounds
    {
        None,
        Respawn,
        Strike,
        Idle,
        Death,
        When_Hit
    }


    [SerializeField]
    float _damageLength = 2f;

    [SerializeField]
    ParticleSystem _HitPartical;

    bool spawnDone = false;


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
        Move(_rigidbody, _speed, _moveLeftSide);
        if (_distance <= _visibility && CanMove())
        {
            if (!spawnDone)
            {
                audioManager.Play(GuardSounds.Respawn.ToString());
                spawnDone = true;
            }
            else if (!audioManager.IsPlaying(GuardSounds.Idle.ToString()))
                audioManager.Play(GuardSounds.Idle.ToString());





            _anima.SetTrigger("Spawn");
        }
    }

    protected override void DoAttack()
    {
        float currentDeltaAttack = Time.fixedTime - _lastAttackTime;
        if ((_distance <= _deltaDistanceAttack) && !_attacks && currentDeltaAttack > _deltaTimeAttack && !_reciveDamage)
        {
            audioManager.Play(GuardSounds.Strike.ToString());
            _attacks = true;
            _anima.SetInteger("Attack Index", UnityEngine.Random.Range(1, 3));
            _anima.SetTrigger("Attack");
            _lastAttackTime = Time.fixedTime;
        }
    }

    public override void TakeHit(float damage)
    {
        _reciveDamage = true;
        _enemyManager._HP -= damage;
        if (_enemyManager._HP <= 0)
            return;

        _HitPartical.Play();
        //StartCoroutine(DoTakeHit(0.5f));
        _reciveDamage = false;
    }

    private IEnumerator DoTakeHit(float time)
    {
        _HitPartical.Play();
        audioManager.Play(GuardSounds.When_Hit.ToString());
        for (float t = 0; t <= time; t += Time.deltaTime)
        {
            yield return null;
        }
        //_HitPartical.Stop();
    }

    protected override void Die()
    {
        audioManager.Play(GuardSounds.Death.ToString());
        base.Die();
    }



}
