using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

		#region Singleton

		public static SceneController instance;

		void Awake()
		{
				instance = this;
		}

		#endregion


		public bool _isLoaded = false;

		public void LoadNextScene()
		{
				var count = PlayerPrefs.GetInt("CountChangeLevel");
				count++;
				PlayerPrefs.SetInt("CountChangeLevel", count);

				int level = PlayerPrefs.GetInt("CompletedLVL");
				Debug.Log(SceneManager.GetActiveScene().name);
				if (SceneManager.GetActiveScene().name == "Respawn")
						PlayerPrefs.SetInt("NextLVL", level);
				else
				{
                    level++;
						PlayerPrefs.SetInt("CompletedLVL", level);
						PlayerPrefs.SetInt("NextLVL", level);
				}

				StartCoroutine(AsyncLoad());
		}


		IEnumerator AsyncLoad()
		{
				yield return new WaitForSeconds(1);
				AsyncOperation operation = SceneManager.LoadSceneAsync("Loading");
				operation.allowSceneActivation = false;

				while (!operation.isDone)
				{
						if (operation.progress == 0.9f)
						{
								if (UIController.instance.ad.IsLoaded() && PlayerPrefs.GetInt("CountChangeLevel") == 3)
								{
										UIController.instance.ad.Show();
										PlayerPrefs.SetInt("CountChangeLevel", 0);
								}
								if (_isLoaded)
										operation.allowSceneActivation = true;
						}
						yield return null;
				}
		}
}
