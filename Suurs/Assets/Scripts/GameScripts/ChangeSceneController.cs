using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[RequireComponent(typeof(PlayableDirector))]
public class ChangeSceneController : MonoBehaviour
{
    PlayableDirector _director;


    private void Start()
    {
        _director = GetComponent<PlayableDirector>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _director.Play();
            SceneController.instance.LoadNextScene();
            SceneController.instance._isLoaded = true;
        }
    }
}
