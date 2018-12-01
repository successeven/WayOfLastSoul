using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LaunchVideo : MonoBehaviour
{
    public string _nextSceneName;
    public bool _canSkip = false;
    public GameObject textToShow;

    bool _finishLoad = false;
    bool _MoveEnd = false;
    bool _TapSkip = false;

    void Start()
    {
        StartCoroutine(AsyncLoad());
        var video = GetComponentInChildren<VideoPlayer>();
        video.loopPointReached += EndReached;
        textToShow.SetActive(false);
				video.Play();
    }

    void EndReached(VideoPlayer vp)
    {
        _MoveEnd = true;
    }

    public void SkipVideo()
    {
        Debug.Log("Клик блять");
        _TapSkip = true;
    }


    IEnumerator AsyncLoad()
    {
        yield return new WaitForSeconds(1);

        AsyncOperation operation = SceneManager.LoadSceneAsync(_nextSceneName);

        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            if (operation.progress == 0.9f)
            {
                _finishLoad = true;
                if (_canSkip)
                  textToShow.SetActive(true);

                if (_MoveEnd || _TapSkip)
                    operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}