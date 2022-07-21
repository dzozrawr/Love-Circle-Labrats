using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContestantQuestioningManager : MonoBehaviour
{
    private static ContestantQuestioningManager instance = null;
    public static ContestantQuestioningManager Instance { get => instance;  }
    

    public List<ContestantScript> contestants;

    private int numberOfContestants = 0;
    private ContestantScript curContestant = null;
    public ContestantScript CurContestant { get => curContestant; set => curContestant = value; }
    private int curContestantInd = 0;



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

    }

    public void StartPhase()
    {
        curContestant = contestants[0];
        curContestantInd = 0;

        CameraController.Instance.transitionToCMVirtualCamera(curContestant.cam); //transition camera to him to start the dialogue
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
            //move to deciding phase
        }
    }
    /*    // Update is called once per frame
        void Update()
        {

        }*/
}
