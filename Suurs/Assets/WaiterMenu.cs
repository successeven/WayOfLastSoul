using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaiterMenu : MonoBehaviour {

    public string _nextSceneName;
    bool _finishLoad = false;
    float startScene;
    void Start()
    {
        StartCoroutine(AsyncLoad());
        startScene = Time.fixedTime;
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
	
	// Update is called once per frame
	void Update () {

        if (Time.fixedTime - startScene > 7f)
            _finishLoad = true;

    }
}
