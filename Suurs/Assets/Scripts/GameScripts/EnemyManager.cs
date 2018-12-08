using System;
using System.Collections;
using UnityEngine;

public class EnemyManager : AliveObject {

    public float _attack;

    bool _DealDamage = false;

    [NonSerialized]
    public bool _death = false;

		[NonSerialized]
		public bool _reciveDamage = false;

		[NonSerialized]
		public bool _HeroEnter = false;

		protected int _dealAttackID;

    private void Start () {
        SetStartSkills ();
    }

    private void FixedUpdate () {
        if (_HP <= 0 && !_death) {
            _death = true;
            Death ();
            Invoke ("Die", 3f);
        }
    }

    protected virtual void SetStartSkills () { }


    void OnTriggerEnter2D (Collider2D collision) 
    {
        if (collision.tag == "Player" ) 
        {
						_HeroEnter = true;
						if (IsAttack())
								if (!_DealDamage) {
										_DealDamage = true;
										Hero.instance.Manager.TakeDamage (_attack);
								}
        }
    }

		private void OnTriggerExit2D(Collider2D collision)
		{
				if (collision.tag == "Player")
						_HeroEnter = false;
		}

		protected virtual bool IsAttack () 
    {
        return false;
    }

    public void ResetEnemyDealAttack () 
    {
        _DealDamage = false;
    }

    public void ResetEnemyReciveAttack () 
    {
        _reciveDamage = false;
        _dealAttackID = 0;
    }




}