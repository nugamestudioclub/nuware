using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestSceneLoad : MonoBehaviour
{
    public string SceneName = "";

    public void LoadScene()
    {
        var operation = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Single);

        StartCoroutine(IEPostLoad(operation));
    }

    private IEnumerator IEPostLoad(AsyncOperation op)
    {
        yield return new WaitUntil(() => op.isDone);


    }
}
