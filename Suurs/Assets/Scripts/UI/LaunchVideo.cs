using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LaunchVideo : MonoBehaviour
{
		public string _nextSceneName;
		bool _finishLoad = false;

		void Start()
		{
				StartCoroutine(AsyncLoad());
				var video = GetComponentInChildren<VideoPlayer>();
				video.loopPointReached += EndReached;
		}

		void EndReached(VideoPlayer vp)
		{
				_finishLoad = true;
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
								if (_finishLoad)
										operation.allowSceneActivation = true;
						}
						yield return null;
				}
		}
}