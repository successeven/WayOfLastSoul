using UnityEngine;
using UnityEngine.UI;

public class EnergyController : MonoBehaviour
{

	Image _statImage;

	float _curValue;
	float _maxValue;

	void Start()
	{
		_statImage = GetComponent<Image>();
	}

	void Update()
	{
		_statImage.fillAmount = 1f - (Hero.instance.Manager._MaxEnergy - Hero.instance.Manager._Energy) / (float)Hero.instance.Manager._MaxEnergy;
	}
}