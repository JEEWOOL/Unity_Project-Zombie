using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingNextScene : MonoBehaviour
{
    public int sceneNumber = 3;
    public Slider loadingBar;
    public Text loadingText;

    public float manualTime = 0f;
    public float manualTT = 5f;

    private void Update()
    {
        manualTime += Time.deltaTime;

        if (manualTime > manualTT)
        {
            StartCoroutine(TransitionNextScene(sceneNumber));
        }
    }

    IEnumerator TransitionNextScene(int num)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(num);

        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            loadingBar.value = ao.progress;
            loadingText.text = (ao.progress * 100f).ToString() + "%";

            if(ao.progress >= 0.9f)
            {
                ao.allowSceneActivation=true;
            }

            yield return null;
        }
    }
}
