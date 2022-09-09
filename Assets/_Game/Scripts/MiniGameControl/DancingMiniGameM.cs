using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using PixelCrushers.DialogueSystem;
using PathCreation;
using PathCreation.Examples;
using DancingMiniGame;

public class DancingMiniGameM : MiniGame
{
    public GameObject placeForPlayer = null;
    public GameObject[] placeForContestants = null;

    public DancingMiniGameManager dancingMiniGameCanvasM=null;

    //public GameObject[] contestantsDogs = null;

    public CinemachineVirtualCamera dancingContestantsCam = null;


    private GameController gameController = null;
    private FinalEliminationManager finalEliminationManager = null;

    //private Animator dogAnimator0 = null, dogAnimator1 = null;

    private DialogueSystemTrigger dialogueSystemTrigger = null;

    private DialogueSystemEvents dialogueSystemEvents = null;


    private void Awake()
    {
        models.SetActive(false);

        dialogueSystemTrigger = GetComponent<DialogueSystemTrigger>();
    }

    private void Start()
    {
        finalEliminationManager = FinalEliminationManager.Instance;

        dialogueSystemEvents=GetComponent<DialogueSystemEvents>();


        dialogueSystemEvents.conversationEvents.onConversationEnd.AddListener((x)=>finalEliminationManager.StartPhase()); 
    }


    [ContextMenu("InitializeMiniGame")]
    public override void InitializeMiniGame()
    {
        models.SetActive(true);
        canvas.gameObject.SetActive(false);
        miniGameCam.gameObject.SetActive(true); //this will be the same

        gameController = GameController.Instance;
        gameController.ContestantsEliminated.AddListener(OnEliminateButtonPressed);
        // gameController.AddListenerForMiniGameEnd(this);

        //dogAnimator0 = contestantsDogs[0].GetComponentInChildren<Animator>();
        //dogAnimator1 = contestantsDogs[1].GetComponentInChildren<Animator>();

        FinalEliminationManager.Instance.SetSelectedMiniGame(this);
    }

    protected override void OnEliminateButtonPressed()
    {
        ContestantScript contestant;

        PlayerInMiniGameGO = Instantiate(gameController.ChosenPlayer.playerModel, placeForPlayer.transform.position, placeForPlayer.transform.rotation); //copy player to position
        
        dancingMiniGameCanvasM.dancingAnimator= PlayerInMiniGameGO.GetComponent<Animator>();

        ContestantQuestioningManager contestantQuestioningManager = ContestantQuestioningManager.Instance;

        for (int i = 0; i < placeForContestants.Length; i++)    //copy contestants to positions
        {
            contestant = Instantiate(contestantQuestioningManager.WinningContestants[i], placeForContestants[i].transform.position, placeForContestants[i].transform.rotation);
            contestant.MatchSuccessPoints = contestantQuestioningManager.WinningContestants[i].MatchSuccessPoints;
            finalEliminationManager.contestants.Add(contestant);
        }
    }

    public void TransitionToContestants()
    {
        CameraController.Instance.transitionToCMVirtualCamera(dancingContestantsCam);
        CheckForCameraBlending.onCameraBlendFinished += ActionWhenCameraOnContestants;
    }

    public void ActionWhenCameraOnContestants()
    {
        //dogAnimator0.SetTrigger("Spin");
        //dogAnimator1.SetTrigger("Bark");
        finalEliminationManager.contestants[0].GetComponentInChildren<Animator>().SetTrigger("Dance");
        finalEliminationManager.contestants[1].GetComponentInChildren<Animator>().SetTrigger("FailDance");

        finalEliminationManager.contestants[0].MatchSuccessPoints++;

        StartCoroutine(WaitForIdle());

        CheckForCameraBlending.onCameraBlendFinished -= ActionWhenCameraOnContestants;
    }

    IEnumerator WaitForIdle(bool isMiniGameOver = false)
    {
        yield return new WaitUntil(() => !finalEliminationManager.contestants[0].GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"));
        //System.Func<bool> a=dogAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle");
        yield return new WaitUntil(() => finalEliminationManager.contestants[0].GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"));
        dialogueSystemTrigger.enabled = true;
//        Debug.Log("dialogueSystemTrigger.enabled");
    }
}
