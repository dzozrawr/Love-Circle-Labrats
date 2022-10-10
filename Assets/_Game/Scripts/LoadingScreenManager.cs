using System.Collections;
using System.Collections.Generic;
using Tabtale.TTPlugins;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenManager : MonoBehaviour
{
    public Text progressText = null;

    public Slider slider = null;

    public float timeToWaitBeforeLoading = 1.5f;

    private AsyncOperation operation;
    private Canvas canvas;

    private int levelToLoad;

    private float loadingTime = 0f;

    private void Awake()
    {

        TTPCore.Setup();

        SaveData saveData = SaveSystem.LoadGame();
        if (saveData != null)
        {
            levelToLoad = saveData.level;
            GameController.CoinAmount = saveData.coins;
            GameController.UnchosenPlayerPrefab = (GameObject)Resources.InstanceIDToObject(saveData.unchosenPlayerPrefabInstanceID);
            GameController.missionID = saveData.missionID;
            SetShop.setsInShopInfos=saveData.setsInShopInfos;   

            Resources.UnloadUnusedAssets();
            // GameController.UnchosenPlayerPrefab=saveData.unchosenPlayerPrefab;
            //GameObject.

        }
        else
        {
            levelToLoad = SceneManager.GetActiveScene().buildIndex + 1;
            GameController.missionID = 1;
        }
    }

    private void Start()
    {
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 30;

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
        operation.allowSceneActivation = false;

        while (!operation.isDone && (loadingTime < timeToWaitBeforeLoading))
        {
            //UpdateProgressUI(operation.progress);
            yield return null;
            loadingTime += Time.deltaTime;
        }
        operation.allowSceneActivation = true;
        // UpdateProgressUI(operation.progress);
        operation = null;
    }

    private void UpdateProgressUI(float progress)
    {
        slider.value = progress;
        progressText.text = (int)(progress * 100f) + "%";
    }
}
