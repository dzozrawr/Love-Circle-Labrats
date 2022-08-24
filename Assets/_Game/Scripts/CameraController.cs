using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private static CameraController instance = null;
    public static CameraController Instance { get => instance; }

    public enum CameraPhase
    {
        Intro, PlayerPicking, ContestantsStart, ContestantsElimination, DogMiniGame, BakingMiniGame
    }

    public Dictionary<CameraPhase, CinemachineVirtualCamera> camerasDictionary = new Dictionary<CameraPhase, CinemachineVirtualCamera>();

    [System.Serializable]
    public class Container
    {
        public CameraPhase phase;
        public CinemachineVirtualCamera cam;
    }

    /*    public CinemachineVirtualCamera introCam = null;
        public CinemachineVirtualCamera playerPickingCam = null;
        public CinemachineVirtualCamera contestantsCam = null;
        public CinemachineVirtualCamera contestantsEliminationCam = null;*/

    [NonReorderable]
    public List<Container> camerasList;
    private int highestCameraPriority = -1;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        int curCamPriority = highestCameraPriority;
        foreach (var item in camerasList)
        {
            curCamPriority = item.cam.GetComponent<CinemachineVirtualCamera>().Priority;
            if (curCamPriority > highestCameraPriority)
            {
                highestCameraPriority = curCamPriority;
            }
            camerasDictionary.Add(item.phase, item.cam.GetComponent<CinemachineVirtualCamera>());
        }
        //here should be a loop going through the cameras determining what is the highest priority number
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    public void transitionToCMVirtualCamera(CameraPhase phase)
    {
        highestCameraPriority = camerasDictionary[phase].Priority = highestCameraPriority + 1;
        Time.timeScale = 1;
    }

    public void transitionToCMVirtualCamera(CinemachineVirtualCamera cam)
    {
        highestCameraPriority = cam.Priority = highestCameraPriority + 1;
        highestCameraPriority = cam.Priority = highestCameraPriority + 1;
    }
}
