using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[RequireComponent(typeof(PlayableDirector))]
public class ChangeSceneController : MonoBehaviour
{

    [SerializeField]
    int _nextLVL = -1;

    [SerializeField]
    bool _finishLVL = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManager.instance.PlayFromTimelines(GameTimeLines.ChangeScene);
            SceneController.instance.LoadNextScene(_nextLVL, _finishLVL);
            SceneController.instance._isLoaded = true;
        }
    }
}
