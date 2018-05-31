using System;
using UnityEngine;
using UnityEngine.UI;

public class AttackTimeController : MonoBehaviour
{
	HeroController _heroController;
	Image _rollTimeImage;

	void Start()
	{
		_heroController = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroController>();
		_rollTimeImage = GetComponent<Image>();
	}

	void LateUpdate()
	{/*
		if (!_heroController._holdAttack)
		{
			_rollTimeImage.fillAmount = 0;
			return;
		}
		float deltaAttack = Time.fixedTime - _heroController._lastAttackTime;
		_rollTimeImage.fillAmount = 1f - (_heroController._deltaRapiraTime - deltaAttack) / (float)_heroController._deltaRapiraTime;*/
	}
}