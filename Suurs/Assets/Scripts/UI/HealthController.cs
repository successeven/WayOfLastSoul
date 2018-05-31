
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{

	HeroManager _heroManager;
	Image _statImage;

	void Start()
	{
		_heroManager = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroManager>();
		_statImage = GetComponent<Image>();
	}

	void Update()
	{
		_statImage.fillAmount = 1f - (_heroManager._MaxHealth - _heroManager._Health) / (float)_heroManager._MaxHealth;
	}
}
