using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaintIn3D;
using UnityEngine.UI;

public class MakeUpMiniGameCanvas : MonoBehaviour
{
    public ProgressBar progressBar = null;



    // Update is called once per frame
    private void Start()
    {
        
        Canvas canvas = GetComponent<Canvas>();
        if (canvas.worldCamera == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag("CameraUI");
            if (go != null)
            {
                canvas.worldCamera = go.GetComponent<Camera>();
            }
        }
    }
}
