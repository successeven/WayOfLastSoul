using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(CrowController))]
public class CrowManager : EnemyManager
{
    CrowController _controller;
    AudioManager _audioManager;

    private void Start() {      
        _audioManager = GetComponent<AudioManager> ();
    }
    protected override void SetStartSkills()
    {/*
				if (PlayerPrefs.HasKey("CrowHP"))
						_HP = PlayerPrefs.GetInt("CrowHP");

				if (PlayerPrefs.HasKey("CrowAttack"))
						_attack = PlayerPrefs.GetInt("CrowAttack");

				if (PlayerPrefs.HasKey("CrowSpeed"))
				{
						var unit = GetComponent<Unit>();
						unit.SetSpeed(PlayerPrefs.GetInt("CrowSpeed"));
				}
				
        int playerLVL = Hero.instance.Manager._Level;

        float timeGame = GameObject.FindGameObjectWithTag("ScriptManager").GetComponent<TimeManager>()._mainTime;
        int complexity = GameObject.FindGameObjectWithTag("ScriptManager").GetComponent<GemerationLevel>()._Сomplexity;

        int delta = (int)Math.Truncate((complexity * 10) + (10 * timeGame) + (playerLVL / 2));
        _attack += delta;
        _HP += (int)Math.Truncate(delta * 1.5f);
				*/
        _controller = GetComponent<CrowController>();
    }
    public override void TakeHit (float damage, int attackID) {
        if (attackID != _dealAttackID) {
            _dealAttackID = attackID;
            _reciveDamage = true;
            //_audioManager.Play (FallenSounds.When_Hit.ToString ());
            _HP -= damage;
           /*  if (_HP <= 0)
                return;
            _anima.SetTrigger ("TakeHit");*/
        }
    }
    protected override bool IsAttack()
    {
        return _controller.StateCrow == StateCrowEnum.Attack;
    }
}
