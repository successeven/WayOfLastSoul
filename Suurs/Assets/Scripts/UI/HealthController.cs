
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{

		Image _statImage;

		void Start()
		{
				_statImage = GetComponent<Image>();
		}

		void Update()
		{
				_statImage.fillAmount = 1f - (Hero.instance.Manager._MaxHealth - Hero.instance.Manager._Health) / (float)Hero.instance.Manager._MaxHealth;
		}
}
