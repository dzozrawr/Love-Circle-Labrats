using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PaintIn3D;
using PixelCrushers.DialogueSystem;
using Cinemachine;
using Contestant;

public class MakeupMiniGame : MiniGame
{
    [System.Serializable]
    public class ContestantMaterialContainer
    {
        public ContestantModelType type;
        public Material mat;
    }

    //public Transform placeForPlayer=null;
    public GameObject playerModelInMinigame = null;

    public P3dChannelCounter channelCounter = null;

    [Range(0, 262144)]
    public int greenBlueCounterFinishValue = 242144;

    public MakeUpMiniGameCanvas makeUpMiniGameCanvas = null;

    public P3dPaintSphere p3DPaintSphere = null;

    public GameObject[] placeForContestants = null;

    public CinemachineVirtualCamera contestantsResultsCam = null;

    [NonReorderable]
    public List<ContestantMaterialContainer> contestantGoodLipstickMatsList = new List<ContestantMaterialContainer>();
    [NonReorderable]
    public List<ContestantMaterialContainer> contestantBadLipstickMatsList = new List<ContestantMaterialContainer>();

    public Dictionary<ContestantModelType, Material> contestantGoodLipstickMatsDict = new Dictionary<ContestantModelType, Material>();
    public Dictionary<ContestantModelType, Material> contestantBadLipstickMatsDict = new Dictionary<ContestantModelType, Material>();

    public Material girlGoodLipstickMat = null;

    private GameController gameController;

    private Slider progressBarSlider = null;

    private float progress = 0f;

    private bool isMiniGameActive = false;

    private FinalEliminationManager finalEliminationManager = null;
    private DialogueSystemTrigger dialogueSystemTrigger = null;

    private DialogueSystemEvents dialogueSystemEvents = null;

    private int greenBlueCounterStartValue;


    private void Awake()
    {
        models.SetActive(false);
        playerModelInMinigame.SetActive(false);

        dialogueSystemTrigger = GetComponent<DialogueSystemTrigger>();

        foreach (ContestantMaterialContainer c in contestantGoodLipstickMatsList)
        {
            contestantGoodLipstickMatsDict.Add(c.type, c.mat);
        }

        foreach (ContestantMaterialContainer c in contestantBadLipstickMatsList)
        {
            contestantBadLipstickMatsDict.Add(c.type, c.mat);
        }
    }

    private void Start()
    {

        finalEliminationManager = FinalEliminationManager.Instance;

        dialogueSystemEvents = GetComponent<DialogueSystemEvents>();


        dialogueSystemEvents.conversationEvents.onConversationEnd.AddListener((x) => finalEliminationManager.StartPhase());

        
       
    }


    public override void InitializeMiniGame()
    {
        models.SetActive(true);
        canvas.gameObject.SetActive(false);
        miniGameCam.gameObject.SetActive(true); //this will be the same

        p3DPaintSphere.gameObject.SetActive(false);

        gameController = GameController.Instance;
        gameController.ContestantsEliminated.AddListener(OnEliminateButtonPressed);

        progressBarSlider = makeUpMiniGameCanvas.progressBarSlider;
        progressBarSlider.value = 0f;
        progressBarSlider.maxValue = 1f;

        FinalEliminationManager.Instance.SetSelectedMiniGame(this);
    }

    protected override void OnEliminateButtonPressed()
    {
        ContestantScript contestant;

        playerModelInMinigame.SetActive(true);

        PlayerInMiniGameGO = gameController.ChosenPlayer.playerModel;
        // PlayerInMiniGameGO = Instantiate(gameController.ChosenPlayer.playerModel, placeForPlayer.transform.position, placeForPlayer.transform.rotation); //copy player to position



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
        //set lipstick materials to contestants
        ContestantScript winnerContestant = finalEliminationManager.contestants[0], loserContenstant = finalEliminationManager.contestants[1];

        winnerContestant.GetComponentInChildren<ToonModelScript>().SetHeadMainMaterial(contestantGoodLipstickMatsDict[winnerContestant.contestantModelType]);
        loserContenstant.GetComponentInChildren<ToonModelScript>().SetHeadMainMaterial(contestantBadLipstickMatsDict[loserContenstant.contestantModelType]);

        CameraController.Instance.transitionToCMVirtualCamera(contestantsResultsCam);
        CheckForCameraBlending.onCameraBlendFinished += ActionWhenCameraOnContestants;
    }

    public void ActionWhenCameraOnContestants()
    {
        //dogAnimator0.SetTrigger("Spin");
        //dogAnimator1.SetTrigger("Bark");
        //  finalEliminationManager.contestants[0].GetComponentInChildren<Animator>().SetTrigger("Happy");
        //  finalEliminationManager.contestants[1].GetComponentInChildren<Animator>().SetTrigger("Sad");

        finalEliminationManager.contestants[0].MatchSuccessPoints++;

        ToonModelScript playerScriptModel = PlayerInMiniGameGO.GetComponentInChildren<ToonModelScript>();
        //Debug.Log(playerScriptModel);
        PlayerInMiniGameGO.GetComponentInChildren<ToonModelScript>().SetHeadMainMaterial(girlGoodLipstickMat);

        Invoke(nameof(StartFinalEliminationConversation), 1.5f);
        //   StartCoroutine(WaitForIdle());   ovde sam stao pre nego sto je god emperor branima stigao
        CheckForCameraBlending.onCameraBlendFinished -= ActionWhenCameraOnContestants;
    }

    private void StartFinalEliminationConversation()
    {
        dialogueSystemTrigger.enabled = true;
    }

    public void TriggerMiniGame()
    {
        p3DPaintSphere.gameObject.SetActive(true);
        makeUpMiniGameCanvas.gameObject.SetActive(true);

        isMiniGameActive = true;

        greenBlueCounterStartValue = channelCounter.CountG;
//        Debug.Log(greenBlueCounterStartValue);
    }

    private void Update()
    {
        if (!isMiniGameActive) return;
        if (Input.GetMouseButton(0))
        {
            progress = ((float)((greenBlueCounterStartValue-greenBlueCounterFinishValue) - (channelCounter.CountG - greenBlueCounterFinishValue))) / ((float)(greenBlueCounterStartValue - greenBlueCounterFinishValue));

            if (progress > 1f) progress = 1f;
           // Debug.Log(progress);
          //   Debug.Log(((greenBlueCounterStartValue-greenBlueCounterFinishValue)-(channelCounter.CountG-greenBlueCounterFinishValue)));//259772
         //    Debug.Log((greenBlueCounterStartValue-greenBlueCounterFinishValue));//2372

            progressBarSlider.value = progress;

            if (progress >= 1f)
            {
                p3DPaintSphere.gameObject.SetActive(false);
                //lipstick model disable
                makeUpMiniGameCanvas.gameObject.SetActive(false);
                isMiniGameActive = false;

                Invoke(nameof(TransitionToContestants),1f);
                //Debug.Log("End mini game");
            }
        }
    }
}
