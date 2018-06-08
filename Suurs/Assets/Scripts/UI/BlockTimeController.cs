using System;
using UnityEngine;
using UnityEngine.UI;

public class BlockTimeController : MonoBehaviour {

	HeroManager _heroManager;
	HeroMotor _heroMotor;
	Image _rollTimeImage;

	void Start()
	{
		_heroManager = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroManager>();
		_heroMotor = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroMotor>();
		_rollTimeImage = GetComponent<Image>();
	}

	void LateUpdate()
	{
		int deltaJump = (int)Math.Truncate((Time.fixedTime - _heroMotor._lastBack_SlideTime) * 1000);
		_rollTimeImage.fillAmount = 1f - (_heroManager._DeltaBack_Slide - deltaJump) / (float)_heroManager._DeltaBack_Slide;
	}
}
