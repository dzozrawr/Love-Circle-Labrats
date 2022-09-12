using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Contestant;

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

    private List<ContestantScript> winningContestants = new List<ContestantScript>();
    public List<ContestantScript> WinningContestants { get => winningContestants; }

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

        GameCanvasController.Instance.eliminateButton.GetComponent<Button>().onClick.AddListener(EliminateSelectedContestants);
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
        /*         for (int i = 0; i < contestants.Count; i++)
                {
                    if (contestants[i].IsSelected)
                    {
                        contestants[i].Eliminate();
                       // contestants.Remove(contestants[i]);
                    }
                } */
        foreach (ContestantScript c in contestants)
        {
            c.thumbsUpOrDownImage.gameObject.SetActive(false);
            if (c.IsSelected)
            {
                c.Eliminate();
            }
            else
            {
                winningContestants.Add(c);
            }
        }
        isSelectionPhaseActive = false;
        numberOfSelectedContestants = 0;

        GameCanvasController.Instance.ToggleEliminateButtonVisibility(false);

        GameController.Instance.ContestantsEliminated?.Invoke();
    }

    public void ContestantEliminatedSignal()
    {
        
        eliminatedContestantsN++;
//        Debug.Log("ContestantEliminatedSignal() eliminatedContestantsN=");
        if (eliminatedContestantsN >= maxContestantsToEliminate)
        {
            GameObject host = GameController.Instance.host;
            Transform placeForHostBeforeMiniGame = GameController.Instance.placeForHostBeforeMiniGame;
            host.transform.position = placeForHostBeforeMiniGame.position;
            host.transform.rotation = placeForHostBeforeMiniGame.rotation;

            cameraController.transitionToCMVirtualCamera(CameraController.CameraPhase.BeforeMiniGame);
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
