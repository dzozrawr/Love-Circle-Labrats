using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PathCreation;
using PathCreation.Examples;
using Contestant;

public class FinalEliminationManager : MonoBehaviour
{
    private static FinalEliminationManager instance = null;
    public List<ContestantScript> contestants;
    private static int maxContestantsToEliminate = 1;

    private Camera mainCamera = null;
    private CameraController cameraController = null;

    public GameObject eliminationOnboardingGroup = null;

    #region Raycast variables
    private Ray ray;
    private RaycastHit hit;
    private GameObject hitObject;
    private bool isSelectionPhaseActive = false;
    #endregion
    private ContestantScript selectedContestant = null;
    private int numberOfSelectedContestants = 0;

    private ContestantScript winnerContestant = null;
    private GameCanvasController gameCanvasController = null;

    private MiniGame selectedMiniGame = null;

    private GameController gameController = null;

    private int winnerContestantInd = 0;

    public static FinalEliminationManager Instance { get => instance; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        gameController = GameController.Instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = CameraController.Instance.GetComponent<Camera>();
        cameraController = CameraController.Instance;
        gameCanvasController = GameCanvasController.Instance;

        
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelectionPhaseActive)
        {
            if(GameController.IsOverRaycastBlockingUI()) return;    //blocks the selection if the UI is in front of it
            if (Input.GetMouseButtonDown(0))
            {
                ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    hitObject = hit.collider.gameObject;
                    if (hitObject.CompareTag("HitboxForElimination"))         //hitObject is a reference to HitBox here
                    {
                        selectedContestant = hitObject.transform.parent.GetComponent<ContestantScript>();

                        if (selectedContestant.IsSelected)    //the function selects (or deselects) contestant and returns the new bool value
                        {
                            selectedContestant.Select();

                            if (numberOfSelectedContestants == maxContestantsToEliminate)
                            {

                                gameCanvasController.ToggleEliminateButtonVisibility(false);
                                eliminationOnboardingGroup.GetComponent<Animation>().Play("EliminationOnboardingAnim");
                            }

                            numberOfSelectedContestants--;
                        }
                        else    //hitting an unselected contestant
                        {

                            if (numberOfSelectedContestants == maxContestantsToEliminate)   //the part where you switch the selected contestant, the other  contestant deselects
                            {
                                selectedContestant.Select();
                                for (int i = 0; i < contestants.Count; i++)
                                {
                                    if (contestants[i] != selectedContestant)
                                    { //deselect the other contestant
                                        contestants[i].Select();
                                    }
                                }

                            }

                            if (numberOfSelectedContestants < maxContestantsToEliminate)
                            {
                                selectedContestant.Select();
                                numberOfSelectedContestants++;

                                if (numberOfSelectedContestants == maxContestantsToEliminate)//the part where you switch the selected contestant, the other  contestant deselects
                                {
                                    if (!gameCanvasController.eliminateButton.gameObject.activeSelf) gameCanvasController.ToggleEliminateButtonVisibility(true);
                                    eliminationOnboardingGroup.GetComponent<Animation>().Play("EliminationOnboardingHide");
                                }
                            }
/*
                            if (numberOfSelectedContestants == maxContestantsToEliminate)   //the part where you switch the selected contestant, the other  contestant deselects
                            {
                                if (!gameCanvasController.eliminateButton.gameObject.activeSelf) gameCanvasController.ToggleEliminateButtonVisibility(true);
                                eliminationOnboardingGroup.GetComponent<Animation>().Play("EliminationOnboardingHide");
                            }*/
                        }
                    }
                }
            }
        }
    }

    private void EliminationEffect()
    {
        foreach (ContestantScript c in contestants)
        {
            c.thumbsUpOrDownImage.gameObject.SetActive(false);
            if (c.IsSelected)
            {
                c.FinalEliminate();
            }
            else
            {
                winnerContestant = c;
                winnerContestantInd = contestants.IndexOf(winnerContestant);
                c.WinnerAction();
            }
        }
        isSelectionPhaseActive = false;
        numberOfSelectedContestants = 0;

        Invoke(nameof(AfterEliminationSequence), 1.5f);

        GameCanvasController.Instance.ToggleEliminateButtonVisibility(false);

        //GameController.Instance.ContestantsEliminated?.Invoke();
    }

    private void AfterEliminationSequence()
    {

        selectedMiniGame.PlayerInMiniGameGO.transform.position = selectedMiniGame.placeForPlayerAfterFinalElim.transform.position;
        selectedMiniGame.PlayerInMiniGameGO.transform.rotation = selectedMiniGame.placeForPlayerAfterFinalElim.transform.rotation;

        PathFollower pathFollower = winnerContestant.gameObject.AddComponent<PathFollower>();
        pathFollower.pathCreator = selectedMiniGame.pathsForContestantsAfterFinalElim[winnerContestantInd];
        pathFollower.endOfPathInstruction = EndOfPathInstruction.Stop;
        pathFollower.speed /= 2f;

        winnerContestant.animator.SetTrigger("Walk");
        winnerContestant.cameraFollow.gameObject.SetActive(true);

        cameraController.transitionToCMVirtualCamera(winnerContestant.cameraFollow);
        //winni

    }

    public void ContestantEndOfPathAction()
    {
        winnerContestant.animator.SetTrigger("Idle");
        cameraController.transitionToCMVirtualCamera(selectedMiniGame.EOLPlayerContestantCam);
        CheckForCameraBlending.onCameraBlendFinished += WhenPlayerContestantFinalCameraActive;
    }

    public void WhenPlayerContestantFinalCameraActive()
    {
        switch (winnerContestant.GetMatchSuccessRate())
        {
            case 0f:
                selectedMiniGame.PlayerInMiniGameGO.GetComponent<Animator>().SetTrigger("Cry");
                break;
            case 0.5f:
                selectedMiniGame.PlayerInMiniGameGO.GetComponent<Animator>().SetTrigger("Okay");
                break;
            case 1f:
                selectedMiniGame.PlayerInMiniGameGO.GetComponent<Animator>().SetTrigger("Happy");
                
                break;
        }
        winnerContestant.animator.SetTrigger("Happy");

        Invoke(nameof(ShowEOLScreenAfterDelay), 3f);

        CheckForCameraBlending.onCameraBlendFinished -= WhenPlayerContestantFinalCameraActive;
    }

    private void ShowEOLScreenAfterDelay()
    {
        GameCanvasController.Instance.ActivateEOLScreenBasedOnMatchSuccessRate(winnerContestant.GetMatchSuccessRate());
    }

    public void StartPhase()
    {
        GameCanvasController.Instance.eliminateButton.GetComponent<Button>().onClick.RemoveAllListeners();
        GameCanvasController.Instance.eliminateButton.GetComponent<Button>().onClick.AddListener(EliminationEffect);
        eliminationOnboardingGroup.SetActive(true);
        eliminationOnboardingGroup.GetComponent<Animation>().Play("EliminationOnboardingAnim");

        foreach (ContestantScript c in contestants)
        {
            c.ToggleSelectionPhase(true);
        }
        isSelectionPhaseActive = true;
    }

    public void SetSelectedMiniGame(MiniGame miniGame)  //I really should've made the parent class hold the needed things for after final elim sequence
    {
        selectedMiniGame = miniGame;
    }
}
