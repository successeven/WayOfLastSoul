using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CrowManager : EnemyManager
{    
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
    }

}
