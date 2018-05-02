using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FallenManager : EnemyManager
{
    protected override void SetStartSkills()
    {
        int playerLVL = _heroManager._Level;

        float timeGame = GameObject.FindGameObjectWithTag("ScriptManager").GetComponent<TimeManager>()._mainTime;
        int complexity = GameObject.FindGameObjectWithTag("ScriptManager").GetComponent<GemerationLevel>()._Сomplexity;

        _attack += (int)Math.Truncate((40 * timeGame * complexity) / (2 + playerLVL));
        _HP += (int)Math.Truncate(150 * complexity + 50 * timeGame + timeGame * playerLVL);
    }
}