using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private static CameraController instance = null;
    public static CameraController Instance { get => instance; }

    public enum Phase{ 
        Intro, ContestantElimination
    }
   
    public Dictionary<Phase, CinemachineVirtualCamera> cameras=new Dictionary<Phase, CinemachineVirtualCamera>();

    [System.Serializable]
    public class Container
    {
        public Phase phase;
        public CinemachineVirtualCamera cam;
    }

    public CinemachineVirtualCamera introCam = null;
    public CinemachineVirtualCamera playerPickingCam = null;
    public CinemachineVirtualCamera contestantsCam = null;
    public CinemachineVirtualCamera contestantsEliminationCam = null;

    public List<Container> camerasList;
    private int highestCameraPriority=0;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        foreach (var item in camerasList)
        {
            cameras.Add(item.phase, item.cam.GetComponent<CinemachineVirtualCamera>());
        }
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
}
