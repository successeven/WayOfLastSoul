using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour
{
    string _LevelName;

    [NonSerialized]
    public bool _showNextLVL = false;
    bool _finishLoad = false;

    Animator _animaHide;
    // Use this for initialization
    void Start()
    {
        if (PlayerPrefs.HasKey("NextLVL"))
        {
            int level = PlayerPrefs.GetInt("NextLVL");
            if (level == 4)
                _LevelName = "FinishMove";
            else if (level > 0)
                _LevelName = "Scene_" + level.ToString();
            else
                _LevelName = "Respawn";
        }

        _animaHide = GetComponent<Animator>();
        LoadLVL();
    }

    public void LoadLVL()
    {
        StartCoroutine(AsyncLoad());
    }

    public void AnimaFinish()
    {
        _showNextLVL = true;
    }

    IEnumerator AsyncLoad()
    {
        yield return new WaitForSeconds(1);
        AsyncOperation operation = SceneManager.LoadSceneAsync(_LevelName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            if (operation.progress == 0.9f)
            {
                if (!_finishLoad)
                {
                    _animaHide.enabled = true;
                    _finishLoad = true;
                }
                if (_showNextLVL)
                    operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
