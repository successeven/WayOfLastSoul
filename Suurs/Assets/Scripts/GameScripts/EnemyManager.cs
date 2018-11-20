using System;
using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public float _attack;
    public float _HP;

    bool _DealDamage = false;

    [NonSerialized]
    public bool _death = false;

		[NonSerialized]
		public bool _reciveDamage = false;
    
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

    public virtual void TakeHit (float damage, int attackID)
		{
		}

    void OnTriggerEnter2D (Collider2D collision) 
    {

        if (collision.tag == "Player" && IsAttack ()) 
        {
            if (!_DealDamage) {
                _DealDamage = true;
                Hero.instance.Manager.TakeDamage (_attack);
            }
        }
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

    protected virtual void Death () { }

    protected virtual void Die () {
        Destroy (transform.gameObject);
    }



}