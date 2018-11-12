using UnityEngine;

[RequireComponent (typeof (FallenMotor))]
[RequireComponent (typeof (FallenManager))]
public class FallentController_v2 : MonoBehaviour {

    #region Options

    [SerializeField]
    float _deltaTimeAttack = 2f; //частота атаки в секундах

    [SerializeField]
    float _deltaDistanceAttack = 2f; //Дистанция атаки

    [SerializeField]
    float _visibility = 25; // Расстояние на котором заметит ГГ

    public bool _canAttack = true;
    public bool _canMove = true;
    public bool _canTeleport = true;

    #endregion

    FallenMotor _motor;
    FallenManager _manager;
    RaycastHit2D hit2D;

    protected float _lastAttackTime;
    protected float _distance;
    bool _wasTeleported = false;
    bool _haveGoal = false;

    void Start () {
        _motor = GetComponent<FallenMotor> ();
        _manager = GetComponent<FallenManager> ();
    }

    Vector2 direction;
    void FixedUpdate () {
        if (_manager._HP <= 0 || Hero.instance.Manager._Health <= 0)
            return;

        _distance = Vector2.Distance (transform.position, Hero.instance.transform.position);
        if (_distance <= _visibility) {
            if (!FallenBrain.instance.CheckPosition (gameObject.GetInstanceID()))
                return;

            if (_canTeleport && FallenBrain.instance._leftFallenID == gameObject.GetInstanceID() && _distance < 10) {
                _motor.Teleport ();
                _canTeleport = false;
            }

            if (_distance > _deltaDistanceAttack) {
                var deltaSpeed = 0.9f;
                if (transform.position.x - Hero.instance.transform.position.x > 0)
                    _motor.CurrentHorAxis = -deltaSpeed;
                else
                    _motor.CurrentHorAxis = deltaSpeed;

            } else {
                _motor.CurrentHorAxis = 0;
                //Держим дистанцию на +- 0.5
                /*var deltaDistance = Math.Abs(_distance - _deltaDistanceAttack);
                if (Math.Round(deltaDistance, 1, MidpointRounding.AwayFromZero)  < 0.5f)
                    _motor.CurrentHorAxis = 0;
                else
                {
                    if (transform.position.x - Hero.instance.transform.position.x > 0)
                        _motor.CurrentHorAxis = 0.2f;
                    else
                        _motor.CurrentHorAxis = -0.2f;
                }*/

                if (Time.fixedTime - _lastAttackTime > _deltaTimeAttack) {
                    _lastAttackTime = Time.fixedTime;
                    if (_canAttack)
                        _motor.Attack ();
                }
            }
        }
         else 
         {
            _motor.CurrentHorAxis = 0;
        }

    }

}