using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PaintIn3D;

public class MakeupMiniGame : MiniGame
{

    //public Transform placeForPlayer=null;
    public GameObject playerModelInMinigame = null;

    public P3dChannelCounter channelCounter = null;

    [Range(0, 2432)]
    public int redCounterFinishValue = 2368;

    public MakeUpMiniGameCanvas makeUpMiniGameCanvas=null;

    public P3dPaintSphere p3DPaintSphere=null;

    private GameController gameController;

    private Slider progressBarSlider=null;

    private float progress=0f;

    private bool isMiniGameActive=false;
    private void Awake()
    {
        models.SetActive(false);
        playerModelInMinigame.SetActive(false);
    }
    public override void InitializeMiniGame()
    {
        models.SetActive(true);
        canvas.gameObject.SetActive(false);
        miniGameCam.gameObject.SetActive(true); //this will be the same

        p3DPaintSphere.gameObject.SetActive(false);

        gameController = GameController.Instance;
        gameController.ContestantsEliminated.AddListener(OnEliminateButtonPressed);

        progressBarSlider=makeUpMiniGameCanvas.progressBarSlider;
        progressBarSlider.value=0f;
        progressBarSlider.maxValue=1f;
    }

    protected override void OnEliminateButtonPressed()
    {
        //ContestantScript contestant;

        playerModelInMinigame.SetActive(true);
        //PlayerInMiniGameGO = Instantiate(gameController.ChosenPlayer.playerModel, placeForPlayer.transform.position, placeForPlayer.transform.rotation); //copy player to position



        /*   ContestantQuestioningManager contestantQuestioningManager = ContestantQuestioningManager.Instance;

          for (int i = 0; i < placeForContestants.Length; i++)    //copy contestants to positions
          {
              contestant = Instantiate(contestantQuestioningManager.WinningContestants[i], placeForContestants[i].transform.position, placeForContestants[i].transform.rotation);
              contestant.MatchSuccessPoints=contestantQuestioningManager.WinningContestants[i].MatchSuccessPoints;
              finalEliminationManager.contestants.Add(contestant);
          } */
    }

    public void TriggerMiniGame()
    {
        p3DPaintSphere.gameObject.SetActive(true);
        makeUpMiniGameCanvas.gameObject.SetActive(true);

        isMiniGameActive=true;
    }

    private void Update() {
        if(!isMiniGameActive) return;
        if (Input.GetMouseButton(0))
        {
            progress=((float)channelCounter.CountR)/((float)redCounterFinishValue);

            if(progress>1f) progress=1f;

            progressBarSlider.value = progress;

            if(progress>=1f){                
                p3DPaintSphere.gameObject.SetActive(false);
                //lipstick model disable
                makeUpMiniGameCanvas.gameObject.SetActive(false);
                isMiniGameActive=false;
                Debug.Log("End mini game");
            }
        }
    }
}
