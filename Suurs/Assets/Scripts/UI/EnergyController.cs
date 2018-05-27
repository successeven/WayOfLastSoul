using UnityEngine;
using UnityEngine.UI;

public class EnergyController : MonoBehaviour
{

	HeroManager _heroManager;
	Image _statImage;

	float _curValue;
	float _maxValue;

	void Start()
	{
		_heroManager = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroManager>();
		_statImage = GetComponent<Image>();
	}

	void Update()
	{
		_statImage.fillAmount = 1f - (_heroManager._MaxEnergy - _heroManager._Energy) / (float)_heroManager._MaxEnergy;
	}
}