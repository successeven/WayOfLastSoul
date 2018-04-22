using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    GameObject _Player;
    [SerializeField]
    float _distance;

    Animator _Anima;
    bool _Moving = false;
    bool _Attack = false;

    void Start () {
        _Player = GameObject.FindGameObjectWithTag("Player");
        _Anima = GetComponent<Animator>();
    }
	
	void FixedUpdate () {
        _distance = Vector2.Distance(transform.position, _Player.transform.position);

        if (_distance <= 30)
            _Moving = true;
        else
            _Moving = false;

        if ((_distance <= 1.5f) && !_Attack)
        {
            _Attack = true;
            _Anima.SetTrigger("Attack");
        }        

        if (_Anima.GetBool("Move") != _Moving)
            _Anima.SetBool("Move", _Moving);
        
    }

    void ResetAttack()
    {
        Debug.Log("ResetAttack");
        _Attack = false;
    }
}
