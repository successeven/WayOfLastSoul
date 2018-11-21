using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class ChangeSceneController : MonoBehaviour {

		public PlayableDirector _playableDirector;

        GameManager gameManager;


		private void Start()
		{
            if (_playableDirector == null)
                gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
            if (collision.tag == "Player")
            {
                if (_playableDirector != null)
    				_playableDirector.Play();
                else
                    gameManager.PlayFromTimelines(0);
                    

                SceneController.instance.LoadNextScene();
                SceneController.instance._isLoaded = true;
            }
		}
}
