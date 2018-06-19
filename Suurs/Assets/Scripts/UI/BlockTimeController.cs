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
				int deltaJump = (int)Math.Truncate((Time.fixedTime - Hero.instance.Motor._lastBack_SlideTime) * 1000);
				_rollTimeImage.fillAmount = 1f - (Hero.instance.Manager._DeltaBack_Slide - deltaJump) / (float)Hero.instance.Manager._DeltaBack_Slide;
		}
}
