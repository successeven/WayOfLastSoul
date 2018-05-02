using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CrowManager : EnemyManager
{    
    protected override void SetStartSkills()
    {
        int playerLVL = _heroManager._Level;

        float timeGame = GameObject.FindGameObjectWithTag("ScriptManager").GetComponent<TimeManager>()._mainTime;
        int complexity = GameObject.FindGameObjectWithTag("ScriptManager").GetComponent<GemerationLevel>()._Сomplexity;

        int delta = (int)Math.Truncate((complexity * 10) + (10 * timeGame) + (playerLVL / 2));
        _attack += delta;
        _HP += (int)Math.Truncate(delta * 1.5f);
    }

}
