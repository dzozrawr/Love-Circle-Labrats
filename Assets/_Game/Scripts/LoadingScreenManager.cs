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

    private int levelToLoad;

    private void Awake()
    {
        SaveData saveData = SaveSystem.LoadGame();
        if (saveData != null)
        {
            levelToLoad = saveData.level;
            GameController.CoinAmount=saveData.coins;
            GameController.UnchosenPlayerPrefab= (GameObject)Resources.InstanceIDToObject(saveData.unchosenPlayerPrefabInstanceID);

            Resources.UnloadUnusedAssets();
           // GameController.UnchosenPlayerPrefab=saveData.unchosenPlayerPrefab;
            //GameObject.
           
        }
        else
        {
            levelToLoad = SceneManager.GetActiveScene().buildIndex + 1;
        }
    }

    private void Start()
    {
        LoadScene(levelToLoad);
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
            //UpdateProgressUI(operation.progress);
            yield return null;
        }
        // UpdateProgressUI(operation.progress);
        operation = null;
    }

    private void UpdateProgressUI(float progress)
    {
        slider.value = progress;
        progressText.text = (int)(progress * 100f) + "%";
    }
}
