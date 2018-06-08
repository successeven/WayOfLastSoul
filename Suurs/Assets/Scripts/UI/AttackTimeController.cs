using System;
using UnityEngine;
using UnityEngine.UI;

public class AttackTimeController : MonoBehaviour
{
		HeroMotor _heroMotor;
		HeroController _heroController;
		Image _rollTimeImage;

		void Start()
		{
				_heroMotor = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroMotor>();
				_heroController = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroController>();
				_rollTimeImage = GetComponent<Image>();
		}

		void LateUpdate()
		{
				if (!_heroController._holdAttack)
				{
						_rollTimeImage.fillAmount = 0;
						return;
				}
				float deltaAttack = Time.fixedTime - _heroMotor._lastAttackTime;
				_rollTimeImage.fillAmount = 1f - (_heroMotor._deltaRapiraTime - deltaAttack) / (float)_heroMotor._deltaRapiraTime;
		}
}