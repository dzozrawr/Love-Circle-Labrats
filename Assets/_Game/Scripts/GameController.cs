using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartContestantsPhase();
        }
    }

    public void StartPlayerPicking()
    {
        CameraController.Instance.transitionToCMVirtualCamera(CameraController.CameraPhase.PlayerPicking);
      //  CameraController.Instance.playerPickingCam.Priority = CameraController.Instance.introCam.Priority + 1;
    }

    public void StartContestantsPhase()
    {
        CameraController.Instance.transitionToCMVirtualCamera(CameraController.CameraPhase.ContestantsStart);
        //CameraController.Instance.contestantsCam.Priority = CameraController.Instance.playerPickingCam.Priority + 1;
    }
}
