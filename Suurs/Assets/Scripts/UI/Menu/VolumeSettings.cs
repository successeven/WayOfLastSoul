using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{

		public Slider slider;
		public Text value;

		// Update is called once per frame
		void Update()
		{
				value.text = slider.value.ToString();
				AudioListener.volume = slider.value;
		}
}
