using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PathCreation;
using PathCreation.Examples;

public class FinalEliminationManager : MonoBehaviour
{
    private static FinalEliminationManager instance = null;
    public List<ContestantScript> contestants;
    private static int maxContestantsToEliminate = 1;

    private Camera mainCamera = null;
    private CameraController cameraController = null;

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

    private DogMiniGameM dogMiniGameM = null;
    private BakingMiniGame bakingMiniGame = null;

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
                            }

                            if (numberOfSelectedContestants == maxContestantsToEliminate)   //the part where you switch the selected contestant, the other  contestant deselects
                            {
                                if (!gameCanvasController.eliminateButton.gameObject.activeSelf) gameCanvasController.ToggleEliminateButtonVisibility(true);                               
                            }
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
                winnerContestantInd= contestants.IndexOf(winnerContestant);
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
        if (dogMiniGameM != null)
        {
            gameController.ChosenPlayer.playerModel.transform.position = dogMiniGameM.placeForPlayerAfterFinalElim.transform.position;
            gameController.ChosenPlayer.playerModel.transform.rotation = dogMiniGameM.placeForPlayerAfterFinalElim.transform.rotation;

            PathFollower pathFollower= winnerContestant.gameObject.AddComponent<PathFollower>();
            pathFollower.pathCreator = dogMiniGameM.pathsForContestantsAfterFinalElim[winnerContestantInd];
            pathFollower.endOfPathInstruction = EndOfPathInstruction.Stop;

            winnerContestant.animator.SetTrigger("Walk");
            //winni
        }
    }

    private void ShowEOLScreenAfterDelay()
    {
        GameCanvasController.Instance.ActivateEOLScreenBasedOnMatchSuccessRate(winnerContestant.GetMatchSuccessRate());
    }

    public void StartPhase()
    {
        GameCanvasController.Instance.eliminateButton.GetComponent<Button>().onClick.RemoveAllListeners();
        GameCanvasController.Instance.eliminateButton.GetComponent<Button>().onClick.AddListener(EliminationEffect);

        foreach (ContestantScript c in contestants)
        {
            c.ToggleSelectionPhase(true);
        }
        isSelectionPhaseActive = true;
    }

    public void SetSelectedMiniGame(MiniGame miniGame)  //I really should've made the parent class hold the needed things for after final elim sequence
    {
        if(miniGame is DogMiniGameM)
        {
            dogMiniGameM = (DogMiniGameM)miniGame;
        }

        if(miniGame is BakingMiniGame)
        {
            bakingMiniGame = (BakingMiniGame)miniGame;
        }
    }
}
