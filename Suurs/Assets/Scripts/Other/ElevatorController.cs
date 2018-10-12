using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
		[SerializeField]
		float _offsetY;
		
		[SerializeField]
		float _speed;

		[SerializeField]
		float _waitTime;

		[SerializeField]
		GameObject _effects;


		bool _needChange = false;

		Vector3 _target;
		float _changeTime;

		void Start()
		{
				ChangeTarget();
		}


		// Update is called once per frame
		void Update()
		{
				if (transform.position == _target && !_needChange)
				{
						_needChange = true;
						_changeTime = Time.fixedTime;
				}

				if (_needChange && Time.fixedTime - _changeTime > _waitTime)
				{
						ChangeTarget();
						_needChange = false;
				}
				_effects.SetActive(!_needChange);
				transform.position = Vector2.MoveTowards(transform.position, _target, _speed * Time.deltaTime);						
		}

		void ChangeTarget()
		{
				_target = transform.position;
				_offsetY *= -1;
				_target.y += _offsetY;
		}
}
