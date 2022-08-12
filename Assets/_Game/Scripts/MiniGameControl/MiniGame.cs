using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public abstract class MiniGame : MonoBehaviour
{
    public CinemachineVirtualCamera miniGameCam = null;
    public GameObject models = null;
    public Canvas canvas = null;


    public abstract void InitializeMiniGame();

    protected virtual void OnEliminateButtonPressed(){

    }
}
