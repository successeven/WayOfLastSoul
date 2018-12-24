using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class ChangeSceneController : MonoBehaviour
{
    GameManager gameManager;


    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gameManager.PlayFromTimelines(GameTimeLines.ChangeScene);
            SceneController.instance.LoadNextScene();
            SceneController.instance._isLoaded = true;
        }
    }
}
