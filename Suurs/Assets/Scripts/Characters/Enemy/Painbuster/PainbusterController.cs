using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainbusterController : MonoBehaviour {

    [Header ("Свойства черепа")]

    public float _idleSpeed = 2f;
    public float _speed = 10f;

    [SerializeField]
    float _visibility = 25f; // Расстояние на котором заметит ГГ

    public float _distanceAttack = 1f;
    
    [Header ("Время смены траектории")]
    [SerializeField]
    float _changeDirectTimer = 2f;
    
    [Header ("Время \"жизни\" ")]
    [SerializeField]
    float _deathTimer;

    [Header ("Диапазон смещения черепа")]
    
    [SerializeField]
    float _xDiapason = 0;
    
    [SerializeField]
    float _yDiapason = 0;

    //********************************************************** */
    Vector3 _target;
    Vector3 _startPos;
    float _changeDirectionMoveTime;
    bool _death = false;
    Animator _anima;

    void Start() {
        _anima = GetComponent<Animator>();
				_startPos = transform.position;
				_target = new Vector3 (Random.Range (_startPos.x - _xDiapason, _startPos.x + _xDiapason), 
            Random.Range (_startPos.y - _yDiapason, _startPos.y + _yDiapason), 0);
        _changeDirectionMoveTime = _changeDirectTimer;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (_death)
            return;

        if (Vector2.Distance (transform.position, Hero.instance.transform.position) > _visibility) 
        {
            transform.position = Vector3.MoveTowards (transform.position, _target, _idleSpeed * Time.fixedDeltaTime);
            _changeDirectionMoveTime -= Time.deltaTime;
            if (_changeDirectionMoveTime <= 0 || transform.position == _target) {
                _target = new Vector3 (Random.Range (_startPos.x - _xDiapason, _startPos.x + _xDiapason), 
                    Random.Range (_startPos.y - _yDiapason, _startPos.y + _yDiapason), 0);
                _changeDirectionMoveTime = _changeDirectTimer;
            }
        } 
        else
            Move();

    }

    void Move()
		{
				_anima.SetBool("Move", true);
				transform.position = Vector3.MoveTowards (transform.position, Hero.instance.transform.position, _speed * Time.fixedDeltaTime);
        _deathTimer -= Time.deltaTime;
        if (Vector3.Distance (transform.position, Hero.instance.transform.position) <= _distanceAttack)
        {
            Explosion();
            return;
        }

        if ( _deathTimer <= 0) 
        {     
            FallToPieces();
            return;
        }
    }

    void FallToPieces()
		{
				GetComponent<PainbusterManager>().enabled = false;            
				_anima.SetBool("FallToPieces", true);
				_death = true;
        Invoke("Death", 2f);
    }

    void Explosion ()
		{
				_anima.SetBool("Explosion", true);
				_death = true;
        Invoke("Death", 2f);
    }

    void Death()
    {
        Destroy (this.gameObject);
    }
}