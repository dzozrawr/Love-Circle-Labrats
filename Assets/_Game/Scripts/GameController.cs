using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameController : MonoBehaviour
{
    public CinemachineVirtualCamera introCam = null;
    public CinemachineVirtualCamera playerPickingCam = null;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartPlayerPicking()
    {
        playerPickingCam.Priority = introCam.Priority + 1;
    }
}
