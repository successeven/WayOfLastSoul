﻿using UnityEngine;

[RequireComponent(typeof(PainbusterManager))]
public class PainbusterController : MonoBehaviour
{

		PainbusterManager _manager;

		[Header("Свойства черепа")]
		public float _idleSpeed = 2f;
		public float _speed = 10f;

		[SerializeField]
		float _visibility = 25f; // Расстояние на котором заметит ГГ

		public float _distanceAttack = 1f;

		[Header("Время смены траектории")]
		[SerializeField]
		float _changeDirectTimer = 2f;

		[Header("Время \"жизни\" ")]
		[SerializeField]
		float _deathTimer;

		[Header("Диапазон смещения черепа")]

		[SerializeField]
		float _xDiapason = 0;

		[SerializeField]
		float _yDiapason = 0;

		//********************************************************** */
		Vector3 _target;
		Vector3 _startPos;
		float _changeDirectionMoveTime;

		void Start()
		{
				_manager = GetComponent<PainbusterManager>();
				_startPos = transform.position;
				_target = new Vector3(Random.Range(_startPos.x - _xDiapason, _startPos.x + _xDiapason),
						Random.Range(_startPos.y - _yDiapason, _startPos.y + _yDiapason), 0);
				_changeDirectionMoveTime = _changeDirectTimer;
		}

		void Update()
		{
				if (_manager._dead || !_manager._canMove)
						return;

				if (Vector2.Distance(transform.position, Hero.instance.transform.position) > _visibility)
				{
						transform.position = Vector3.MoveTowards(transform.position, _target, _idleSpeed * Time.fixedDeltaTime);
						_changeDirectionMoveTime -= Time.deltaTime;
						if (_changeDirectionMoveTime <= 0 || transform.position == _target)
						{
								_target = new Vector3(Random.Range(_startPos.x - _xDiapason, _startPos.x + _xDiapason),
										Random.Range(_startPos.y - _yDiapason, _startPos.y + _yDiapason), 0);
								_changeDirectionMoveTime = _changeDirectTimer;
						}
				}
				else
				{
						_manager._anima.SetBool("Move", true);
						if (!_manager._audioManager.IsPlaying(AudioPainbuster.Stalking.ToString()))
								_manager._audioManager.Play(AudioPainbuster.Stalking.ToString());

						transform.position = Vector3.MoveTowards(transform.position, Hero.instance.transform.position, _speed * Time.deltaTime);
						_deathTimer -= Time.deltaTime;					

						if (_deathTimer <= 0)
								_manager.FallToPieces();
				}
		}
}