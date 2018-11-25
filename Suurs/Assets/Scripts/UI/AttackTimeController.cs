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
                if (!Hero.instance.Controller._holdAttack || Hero.instance.Controller._holdAttackTime == -1 
                    || Hero.instance.Motor._attacks )
				{
						_rollTimeImage.fillAmount = 0;
						return;
				}

				float deltaAttack = Time.fixedTime - Hero.instance.Controller._holdAttackTime;
                _rollTimeImage.fillAmount = 1f - (Hero.instance.Motor._shield_AttackTime - deltaAttack) / 
                    (float)Hero.instance.Motor._shield_AttackTime;
		}
}