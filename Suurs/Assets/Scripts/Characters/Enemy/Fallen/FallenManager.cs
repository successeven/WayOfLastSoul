using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FallenManager : MonoBehaviour {

    public int _attack = 40;
    public int _HP = 150;
    [NonSerialized]
    public bool _doAttack = false;

    // Use this for initialization
    void Start () {
        int playerLVL = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroManager>()._Level;
        float timeGame = GameObject.FindGameObjectWithTag("ScriptManager").GetComponent<TimeManager>()._mainTime;
        int complexity = GameObject.FindGameObjectWithTag("ScriptManager").GetComponent<GemerationLevel>()._Сomplexity;


        _attack += (int)Math.Truncate((40 * timeGame * complexity) / (2 + playerLVL));
        _HP += (int)Math.Truncate(150 * complexity + 50 * timeGame + timeGame * playerLVL);
    }
	

    public void ResetDoAttack()
    {
        _doAttack = false;
    }
}
