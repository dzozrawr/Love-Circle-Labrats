using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Canvas))]
public class CanvasFindAndBindUICamera : MonoBehaviour
{
    private Canvas canvas = null;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }
    
    void Start()
    {
        if (canvas.worldCamera == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag("CameraUI");
            if (go != null)
            {
                canvas.worldCamera = go.GetComponent<Camera>();
            }
            else
            {
                Debug.LogError("No CameraUI !!");
            }
        }

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        GameObject go = GameObject.FindGameObjectWithTag("CameraUI");
        if (go != null)
        {
            canvas.worldCamera = go.GetComponent<Camera>();
        }
        else
        {
            Debug.LogError("No CameraUI !!");
        }
    }
}
