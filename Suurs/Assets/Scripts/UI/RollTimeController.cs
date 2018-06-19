using System;
using UnityEngine;
using UnityEngine.UI;

public class RollTimeController : MonoBehaviour
{

		Image _rollTimeImage;

		void Start()
		{
				_rollTimeImage = GetComponent<Image>();
		}

		void LateUpdate()
		{
				int deltaJump = (int)Math.Truncate((Time.fixedTime - Hero.instance.Motor._lastRollTime) * 1000);
				_rollTimeImage.fillAmount = 1f - (Hero.instance.Manager._DeltaRoll - deltaJump) / (float)Hero.instance.Manager._DeltaRoll;
		}
}
