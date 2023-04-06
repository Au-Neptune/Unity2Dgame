using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadingScene : MonoBehaviour
{
    [Range(0, 10)] public int ProgressTime = 2;
    private AsyncOperation _asyncOperation;
    private float ftime = 0f;
    private bool start = false;

    void Start()
    {
        ftime = 0f;
        start = true;
        this.StartCoroutine(this.LoadSceneAsyncProcess("MainPage"));
    }

    void Update()
    {
        if (start)
        {
            ftime += Time.deltaTime;

            if (ftime >= ProgressTime && this._asyncOperation != null)
                this._asyncOperation.allowSceneActivation = true;
        }
    }

    private IEnumerator LoadSceneAsyncProcess(string sceneName)
    {
        // Begin to load the Scene you have specified.
        this._asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        // Don't let the Scene activate until you allow it to.
        this._asyncOperation.allowSceneActivation = false;

        while (!this._asyncOperation.isDone)
        {
            Debug.Log($"[scene]:{sceneName} [load progress]: {this._asyncOperation.progress}");

            yield return null;
        }
    }
}
