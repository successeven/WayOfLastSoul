using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionController : MonoBehaviour {

		public Animator _anima;

		public void HideMenu()
		{
				_anima.SetBool("Options", false);
		}
}
