using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using PathCreation;
using PathCreation.Examples;

public abstract class MiniGame : MonoBehaviour
{
    public CinemachineVirtualCamera miniGameCam = null;
    public GameObject models = null;
    public Canvas canvas = null;
    public PathCreator[] pathsForContestantsAfterFinalElim;
    public GameObject placeForPlayerAfterFinalElim = null;
    public CinemachineVirtualCamera EOLPlayerContestantCam = null;

    protected GameObject playerInMiniGameGO = null;

    public GameObject PlayerInMiniGameGO { get => playerInMiniGameGO; set => playerInMiniGameGO = value; }


    public abstract void InitializeMiniGame();

    protected virtual void OnEliminateButtonPressed(){

    }
}
