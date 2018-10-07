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
		/*		var deltaRoll = Time.fixedTime - Hero.instance.Motor._lastRollTime;
				_rollTimeImage.fillAmount = 1f - (Hero.instance.Manager._DeltaRoll - deltaRoll) / (float)Hero.instance.Manager._DeltaRoll;*/
		}
}
