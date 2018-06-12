using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowValueScript : MonoBehaviour
{
		public string _SettingName;
		public int _defaultValue;
		int _value;

		Text _text;

		// Use this for initialization
		void Start()
		{
				_text = GetComponent<Text>();
				if (PlayerPrefs.HasKey(_SettingName))
						_value = PlayerPrefs.GetInt(_SettingName);
				else
				{
						_value = _defaultValue;
						PlayerPrefs.SetInt(_SettingName, Mathf.RoundToInt(_defaultValue));
				}
				GetComponentInParent<Slider>().value = _value;
				_text.text = "[" + _value.ToString() + "]";
		}

		private void Update()
		{
				_text.text = "[" + _value.ToString() + "]";
		}

		public void TextUpdate(float value)
		{
				_value = Mathf.RoundToInt(value);
				PlayerPrefs.SetInt(_SettingName, Mathf.RoundToInt(value));
		}
}
