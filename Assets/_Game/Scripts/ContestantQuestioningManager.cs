using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContestantQuestioningManager : MonoBehaviour
{
    private static ContestantQuestioningManager instance = null;
    public static ContestantQuestioningManager Instance { get => instance; }

    private static int maxContestantsToEliminate = 3;


    public List<ContestantScript> contestants;


    private int numberOfContestants = 0;
    private ContestantScript curContestant = null;
    public ContestantScript CurContestant { get => curContestant; set => curContestant = value; }

    private int curContestantInd = 0;
    private bool isSelectionPhaseActive = false;
    private Camera mainCamera = null;

    #region Raycast variables
    private Ray ray;
    private RaycastHit hit;
    private GameObject hitObject;
    #endregion
    private ContestantScript selectedContestant = null;
    private int numberOfSelectedContestants = 0;
    private CameraController cameraController = null;

    private int eliminatedContestantsN = 0;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        numberOfContestants = contestants.Count;
    }
    void Start()
    {
        mainCamera = CameraController.Instance.GetComponent<Camera>();
        cameraController = CameraController.Instance;
    }

    private void Update()
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
                                GameCanvasController.Instance.ToggleEliminateButtonVisibility(false);
                            }

                            numberOfSelectedContestants--;
                        }
                        else
                        {
                            if (numberOfSelectedContestants < maxContestantsToEliminate)
                            {
                                selectedContestant.Select();
                                numberOfSelectedContestants++;
                            }

                            if (numberOfSelectedContestants == maxContestantsToEliminate)
                            {
                                GameCanvasController.Instance.ToggleEliminateButtonVisibility(true);
                            }
                        }
                    }
                }
            }
        }
    }

    public void EliminateSelectedContestants()
    {
        foreach (ContestantScript c in contestants)
        {
            if (c.IsSelected) c.Eliminate();
        }
        isSelectionPhaseActive = false;
        numberOfSelectedContestants = 0;

        GameCanvasController.Instance.ToggleEliminateButtonVisibility(false);


    }

    public void ContestantEliminatedSignal()
    {
        eliminatedContestantsN++;
        if (eliminatedContestantsN >= maxContestantsToEliminate)
        {
            cameraController.transitionToCMVirtualCamera(GameController.Instance.ChosenPlayer.miniGame.miniGameCam);
        }
    }



    public void StartContestantsDialoguePhase()
    {
        curContestant = contestants[0];
        curContestantInd = 0;

        CameraController.Instance.transitionToCMVirtualCamera(curContestant.cam); //transition camera to him to start the dialogue
    }

    public void StartContestantsEliminationPhase()
    {
        foreach (ContestantScript c in contestants)
        {
            c.ToggleSelectionPhase(true);
        }
        isSelectionPhaseActive = true;
    }

    public void MoveToNextContestant()
    {
        curContestantInd++;
        if (curContestantInd < contestants.Count)
        {
            curContestant = contestants[curContestantInd];
            CameraController.Instance.transitionToCMVirtualCamera(curContestant.cam);
        }
        else
        {
            CameraController.Instance.transitionToCMVirtualCamera(CameraController.CameraPhase.ContestantsElimination);
        }
    }
    /*    // Update is called once per frame
        void Update()
        {

        }*/
}
