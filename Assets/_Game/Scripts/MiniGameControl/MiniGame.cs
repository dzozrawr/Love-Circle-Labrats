using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using PathCreation;
using PathCreation.Examples;
using UnityEngine.Events;
using Contestant;

public abstract class MiniGame : MonoBehaviour
{
    public CinemachineVirtualCamera miniGameCam = null;
    public GameObject models = null;
    public Canvas canvas = null;
    public PathCreator[] pathsForContestantsAfterFinalElim;
    public GameObject placeForPlayerAfterFinalElim = null;
    public CinemachineVirtualCamera EOLPlayerContestantCam = null;
    public GameObject[] placeForContestants = null;
    public GameObject placeForPlayer = null;

    protected UnityEvent miniGameDone;

    protected GameObject playerInMiniGameGO = null;

    protected GameController gameController = null;

    protected FinalEliminationManager finalEliminationManager = null;

    public GameObject PlayerInMiniGameGO { get => playerInMiniGameGO; set => playerInMiniGameGO = value; }
    public UnityEvent MiniGameDone { get => miniGameDone; set => miniGameDone = value; }



    public virtual void InitializeMiniGame()
    {
        models.SetActive(true);
        canvas.gameObject.SetActive(false);
        miniGameCam.gameObject.SetActive(true); //this will be the same

        gameController = GameController.Instance;
        gameController.ContestantsEliminated.AddListener(OnEliminateButtonPressed);

        FinalEliminationManager.Instance.SetSelectedMiniGame(this);
    }

    protected virtual void Start()
    {
        //gameController = GameController.Instance;
        finalEliminationManager = FinalEliminationManager.Instance;
    }

    protected virtual void OnEliminateButtonPressed(){
        ContestantScript contestant;

        PlayerInMiniGameGO = Instantiate(gameController.ChosenPlayer.playerModel, placeForPlayer.transform.position, placeForPlayer.transform.rotation); //copy player to position



        ContestantQuestioningManager contestantQuestioningManager = ContestantQuestioningManager.Instance;

        for (int i = 0; i < placeForContestants.Length; i++)    //copy contestants to positions
        {
            contestant = Instantiate(contestantQuestioningManager.WinningContestants[i], placeForContestants[i].transform.position, placeForContestants[i].transform.rotation);
            contestant.MatchSuccessPoints = contestantQuestioningManager.WinningContestants[i].MatchSuccessPoints;
            finalEliminationManager.contestants.Add(contestant);
        }
    }
}
