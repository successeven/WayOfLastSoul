﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickSound : MonoBehaviour
{

		public AudioClip sound;

		Button button { get { return GetComponent<Button>(); } }
		AudioSource source { get { return GetComponent<AudioSource>(); } }
		// Use this for initialization
		void Start()
		{
				gameObject.AddComponent<AudioSource>();
				source.clip = sound;
				source.playOnAwake = false;
				button.onClick.AddListener(() => PlaySound());
		}

		void PlaySound()
		{
				source.PlayOneShot(sound);
		}
}
