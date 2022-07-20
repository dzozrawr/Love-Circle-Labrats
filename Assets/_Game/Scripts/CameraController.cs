using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private static CameraController instance = null;
    public static CameraController Instance { get => instance; }

    public CinemachineVirtualCamera introCam = null;
    public CinemachineVirtualCamera playerPickingCam = null;
    public CinemachineVirtualCamera contestantsCam = null;

    private int highestCameraPriority=0;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        //here should be a loop going through the cameras determining what is the highest priority number
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void transitionToCMVirtualCamera(CinemachineVirtualCamera cam)
    {
        cam.Priority += 10;
    }


/*    // Update is called once per frame
    void Update()
    {
        
    }*/
}
