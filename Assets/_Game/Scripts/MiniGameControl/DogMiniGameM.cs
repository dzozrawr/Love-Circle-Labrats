using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using PixelCrushers.DialogueSystem;

public class DogMiniGameM : MiniGame
{
    public GameObject placeForPlayer = null;
    public GameObject[] placeForContestants = null;

    public GameObject[] contestantsDogs = null;

    public CinemachineVirtualCamera dogContestantsCam = null;

    private GameController gameController = null;
    private FinalEliminationManager finalEliminationManager = null;

    private Animator dogAnimator0 = null, dogAnimator1 = null;

    private DialogueSystemTrigger dialogueSystemTrigger=null;

    private void Awake()
    {
        models.SetActive(false);

        dialogueSystemTrigger=GetComponent<DialogueSystemTrigger>();
    }

    private void Start()
    {
        finalEliminationManager = FinalEliminationManager.Instance;
    }


    [ContextMenu("InitializeMiniGame")]
    public override void InitializeMiniGame()
    {
        models.SetActive(true);
        canvas.gameObject.SetActive(false);
        miniGameCam.gameObject.SetActive(true); //this will be the same

        gameController = GameController.Instance;
        gameController.ContestantsEliminated.AddListener(OnEliminateButtonPressed);

        dogAnimator0 = contestantsDogs[0].GetComponentInChildren<Animator>();
        dogAnimator1 = contestantsDogs[1].GetComponentInChildren<Animator>();
    }

    protected override void OnEliminateButtonPressed()
    {
        ContestantScript contestant;
        Instantiate(gameController.ChosenPlayer.playerModel, placeForPlayer.transform.position, placeForPlayer.transform.rotation); //copy player to position
        ContestantQuestioningManager contestantQuestioningManager = ContestantQuestioningManager.Instance;

        for (int i = 0; i < placeForContestants.Length; i++)    //copy contestants to positions
        {
            contestant = Instantiate(contestantQuestioningManager.WinningContestants[i], placeForContestants[i].transform.position, placeForContestants[i].transform.rotation);
            finalEliminationManager.contestants.Add(contestant);
        }
    }

    public void TransitionToContestants()
    {
        CameraController.Instance.transitionToCMVirtualCamera(dogContestantsCam);
        CheckForCameraBlending.onCameraBlendFinished += ActionWhenCameraOnContestants;
    }

    public void ActionWhenCameraOnContestants()
    {
        dogAnimator0.SetTrigger("Spin");
        dogAnimator1.SetTrigger("Bark");

        StartCoroutine(WaitForIdle());

        CheckForCameraBlending.onCameraBlendFinished -= ActionWhenCameraOnContestants;
    }

    IEnumerator WaitForIdle(bool isMiniGameOver = false)
    {
        yield return new WaitUntil(() => !dogAnimator0.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
        //System.Func<bool> a=dogAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle");
        yield return new WaitUntil(() => dogAnimator0.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
        dialogueSystemTrigger.enabled=true;
    }
}
