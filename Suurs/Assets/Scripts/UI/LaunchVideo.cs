using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LaunchVideo : MonoBehaviour
{
    void Start()
    {
				var video = GetComponentInChildren<VideoPlayer>();
				video.loopPointReached += EndReached;
		}

		void EndReached(VideoPlayer vp)
		{
				SceneManager.LoadScene("Menu");
		}
}