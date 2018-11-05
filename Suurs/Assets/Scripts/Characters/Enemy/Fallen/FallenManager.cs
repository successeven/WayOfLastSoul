using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AudioManager))]
[RequireComponent (typeof (FallenMotor))]
public class FallenManager : EnemyManager {
    FallenMotor _motor;
    AudioManager _audioManager;

    protected override void SetStartSkills () {
        _audioManager = GetComponent<AudioManager> ();
        _motor = GetComponent<FallenMotor> ();

    }

    protected override bool IsAttack () {
        return _motor._attacks;
    }

    protected override void Death () {
        FallenBrain.instance.ClearPosition(gameObject.GetInstanceID());
        _audioManager.Play (FallenSounds.Death.ToString ());
    }

    public override void TakeHit (float damage, int attackID) {
        if (attackID != _dealAttackID) {
            _dealAttackID = attackID;
            _reciveDamage = true;
            _audioManager.Play (FallenSounds.When_Hit.ToString ());
            _HP -= damage;
            if (_HP <= 0)
                return;
            _anima.SetTrigger ("TakeHit");
        }
    }
}