using System;
using UnityEngine;
using UnityEngine.UI;

public class BlockTimeController : MonoBehaviour
{
		Image _rollTimeImage;
		void Start()
		{
				_rollTimeImage = GetComponent<Image>();
		}

		void LateUpdate()
		{
				var deltatBack_SlideTime = Time.fixedTime - Hero.instance.Motor._lastDodgeTime;
				_rollTimeImage.fillAmount = 1f - (Hero.instance.Manager._DeltaBack_Slide - deltatBack_SlideTime) / Hero.instance.Manager._DeltaBack_Slide;
		}
}
