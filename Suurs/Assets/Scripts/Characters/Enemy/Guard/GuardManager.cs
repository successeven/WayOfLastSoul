using System.Collections;
using System;
using UnityEngine;

public class GuardManager : EnemyManager
{

		protected override void SetStartSkills()
		{

				//float timeGame = 1;// GameObject.FindGameObjectWithTag("ScriptManager").GetComponent<TimeManager>()._mainTime;
    //    int complexity = GameObject.FindGameObjectWithTag("ScriptManager").GetComponent<GemerationLevel>()._Сomplexity;

				_attack = 40; //	(int)Math.Truncate((40 * timeGame * complexity) / (2 + playerLVL));
				_HP = 300; // (int)Math.Truncate(150 * complexity + 50 * timeGame + timeGame * playerLVL); 
		}
}
