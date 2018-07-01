using System;
using UnityEngine;
using UnityEngine.UI;

public class AttackTimeController : MonoBehaviour
{
		Image _rollTimeImage;

		void Start()
		{
				_rollTimeImage = GetComponent<Image>();
		}

		void LateUpdate()
		{
				if (!Hero.instance.Controller._holdAttack)
				{
						_rollTimeImage.fillAmount = 0;
						return;
				}
				float deltaAttack = Time.fixedTime - Hero.instance.Motor._lastAttackTime;
				_rollTimeImage.fillAmount = 1f - (Hero.instance.Motor._deltaRapiraTime - deltaAttack) / (float)Hero.instance.Motor._deltaRapiraTime;
		}
}