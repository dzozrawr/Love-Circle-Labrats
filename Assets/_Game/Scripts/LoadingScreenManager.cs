using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenManager : MonoBehaviour
{
    public Text progressText = null;

    public Slider slider = null;

    private AsyncOperation operation;
    private Canvas canvas;


    private void Start()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadScene(int sceneIndex)
    {
       // UpdateProgressUI(0);

        StartCoroutine(BeginLoad(sceneIndex));
    }

    private IEnumerator BeginLoad(int sceneIndex)
    {
        operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            UpdateProgressUI(operation.progress);
            yield return null;
        }
        UpdateProgressUI(operation.progress);
        operation = null;
    }

    private void UpdateProgressUI(float progress)
    {
        slider.value = progress;
        progressText.text = (int)(progress * 100f) + "%";
    }
}
