using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DestroyWallScript : AliveObject
{

    public GameObject _hitParticalObject;
    public GameObject _dethParticalObject;
    public float _timeShake = 0.5f;

    [Space(10)]
    public GameObject _Image;
    public float ShakingStrength;

    bool _death = false;
    Vector3 _startPos, _newPos;

    void Start()
    {
        _startPos = _Image.transform.position;
    }

    private void FixedUpdate()
    {
        if (_HP <= 0 && !_death)
        {
            _death = true;
            Death();
            Invoke("Die", 3f);
        }
    }

    protected override void Death()
    {
        _Image.SetActive(false);
        GetComponent<BoxCollider2D>().isTrigger = true;
        if (_dethParticalObject != null)
            Instantiate(_dethParticalObject, transform.position, Quaternion.identity);
    }

    public override void TakeHit(float damage, int attackID)
    {
        if (attackID != _dealAttackID)
        {
            _dealAttackID = attackID;
            _HP -= damage;
            if (_HP <= 0)
                return;

            if (_hitParticalObject != null)
                Instantiate(_hitParticalObject, transform.position, Quaternion.identity);

            if (_Image != null)
                StartCoroutine(DoShake(_timeShake));
        }
    }

    private IEnumerator DoShake(float time)
    {
        for (float t = 0; t <= time; t += Time.fixedDeltaTime)
        {
            _newPos.y = Random.Range(_startPos.y - ShakingStrength, _startPos.y + ShakingStrength);
            _newPos.x = Random.Range(_startPos.x - ShakingStrength, _startPos.x + ShakingStrength);
            _newPos.z = 0;
            _Image.transform.position = _newPos;
            yield return null;
        }
        _dealAttackID = 0;
    }



}
