using UnityEngine;

[RequireComponent(typeof(GhostMotor))]
[RequireComponent(typeof(GhostManager))]
public class GhostController : MonoBehaviour
{
		#region Options

		[SerializeField]
		float _deltaTimeAttack = 2f; //частота атаки в секундах

		[SerializeField]
		float _deltaDistanceAttack = 2f; //Дистанция атаки

		[SerializeField]
		float _visibility = 25f; // Расстояние на котором заметит ГГ

		[SerializeField]
		float _distancePatrol = 15f; // патрулируемое расстояние

		[SerializeField]
		[Range(-0.6f, 0.6f)]
		float _speedIdle;

		[SerializeField]
		[Range(-0.9f, 0.9f)]
		float _speedMove;

		public bool _canAttack = true;
		public bool _canMove = true;

		#endregion

		GhostMotor _motor;
		GhostManager _manager;

		protected float _lastAttackTime;
		protected float _distance;

		Vector3 _startPos;
		bool _patrolLeft = true;


		void Start()
		{
				_motor = GetComponent<GhostMotor>();
				_manager = GetComponent<GhostManager>();
				_startPos = transform.position;
		}

		// Update is called once per frame
		void FixedUpdate()
		{

				if (_manager._HP <= 0 || Hero.instance.Manager._Health <= 0)
						return;

				_distance = Vector3.Distance(transform.position, Hero.instance.transform.position);

				if (_distance <= _visibility && Hero.instance.Motor.CurrentHorAxis > 0.85f)
				{
						_motor._visibleHero = true;
						_manager._isAgressive = true;
				}

				if ((_manager._isAgressive && _distance < _visibility * 2f))
				{
						if (_distance > _deltaDistanceAttack)
						{
								if (Hero.instance.transform.position.x - transform.position.x < 0)
										_motor.CurrentHorAxis = -_speedMove;
								else
										_motor.CurrentHorAxis = _speedMove;
						}
						else if (!_motor._attacks)
						{
								_motor.CurrentHorAxis = 0;
								if (Time.fixedTime - _lastAttackTime > _deltaTimeAttack)
								{
										_lastAttackTime = Time.fixedTime;
										if (_canAttack)
												_motor.Attack();
								}
						}
				}
				else
				{
						_motor._visibleHero = false;
						_manager._isAgressive = false;
						_motor.CurrentHorAxis = _speedIdle;
						if ((transform.position.x < _startPos.x - _distancePatrol && _patrolLeft) ||
								(transform.position.x > _startPos.x + _distancePatrol) && !_patrolLeft)
						{
								_speedIdle *= -1;
								_patrolLeft = !_patrolLeft;
						}
				}
		}
}
