using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class ChangeSceneController : MonoBehaviour {

		PlayableDirector _playableDirector;

		private void Start()
		{
				_playableDirector = GetComponent<PlayableDirector>();
		}


		private void OnTriggerEnter2D(Collider2D collision)
		{
				if (collision.tag == "Player")
				{
						_playableDirector.Play();
						SceneController.instance.LoadNextScene();
						SceneController.instance._isLoaded = true;

				}
		}
}
