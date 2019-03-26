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

    public void LoadNextScene(int NextLVL = -1, bool FinishLVL = false)
    {
        var count = PlayerPrefs.GetInt("CountChangeLevel"); //Количество "сменных" сцен. для рекламы
        count++;
        PlayerPrefs.SetInt("CountChangeLevel", count);

        if (FinishLVL)
        {
            int level = PlayerPrefs.GetInt("CompletedLVL"); //Количество завершенных уровней
            level++;
            PlayerPrefs.SetInt("CompletedLVL", level);
        }

        PlayerPrefs.SetInt("NextLVL", NextLVL);

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
                if (UIController.instance.ad.IsLoaded() && PlayerPrefs.GetInt("CountChangeLevel") == 2)
                {
                    UIController.instance.ad.Show();
                    PlayerPrefs.SetInt("CountChangeLevel", 0);
                }
                //if (_isLoaded)
                    operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
