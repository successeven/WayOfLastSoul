using System;
using UnityEngine;
using UnityEngine.UI;

public class RollTimeController : MonoBehaviour {

	HeroManager _heroManager;
	HeroController _heroController;
	Image _rollTimeImage;

	void Start()
	{
		_heroManager = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroManager>();
		_heroController = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroController>();
		_rollTimeImage = GetComponent<Image>();
	}

	void LateUpdate()
	{
		int deltaJump = (int)Math.Truncate((Time.fixedTime - _heroController._lastRollTime) * 1000);
		_rollTimeImage.fillAmount = 1f - (_heroManager._DeltaRoll - deltaJump) / (float)_heroManager._DeltaRoll;
	}
}
