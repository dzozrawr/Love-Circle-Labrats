using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using PathCreation;
using PathCreation.Examples;
using UnityEngine.Events;

public abstract class MiniGame : MonoBehaviour
{
    public CinemachineVirtualCamera miniGameCam = null;
    public GameObject models = null;
    public Canvas canvas = null;
    public PathCreator[] pathsForContestantsAfterFinalElim;
    public GameObject placeForPlayerAfterFinalElim = null;
    public CinemachineVirtualCamera EOLPlayerContestantCam = null;

    protected UnityEvent miniGameDone;

    protected GameObject playerInMiniGameGO = null;

    public GameObject PlayerInMiniGameGO { get => playerInMiniGameGO; set => playerInMiniGameGO = value; }
    public UnityEvent MiniGameDone { get => miniGameDone; set => miniGameDone = value; }

    public abstract void InitializeMiniGame();

    protected virtual void OnEliminateButtonPressed(){

    }
}
