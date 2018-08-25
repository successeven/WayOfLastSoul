using System.Collections;
using System;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float _attack;
    public float _HP;

    bool _DealDamage = false;


    Animator _anima;
    [NonSerialized]
    public bool _death = false;
    EnemyController _controller;

    private void Start()
    {
        _anima = GetComponent<Animator>();
        _controller = GetComponent<EnemyController>();

        SetStartSkills();
    }

    private void Update()
    {
        if (_HP <= 0 && !_death)
        {
            _death = true;
            _anima.SetTrigger("Death");
            Invoke("DestroyObject", 3f);
        }
    }

    protected virtual void DestroyObject()
    {
        Destroy(transform.root.gameObject);
    }

    protected virtual void SetStartSkills()
    {
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "Player" && _controller._attacks)
        {
            if (!_DealDamage)
            {
                _DealDamage = true;
                Hero.instance.Manager.TakeDamage(_attack);
            }
        }
    }

    public void ResetEnemyDealAttack()
    {
        _DealDamage = false;
    }

}
