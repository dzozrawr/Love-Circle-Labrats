using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContestantQuestioningManager : MonoBehaviour
{
    public List<ContestantScript> contestants;

    private int numberOfContestants = 0;
    private ContestantScript curContestant = null;

    private void Awake()
    {
        numberOfContestants = contestants.Count;
    }
    void Start()
    {
        
    }

    public void StartPhase()
    {
        curContestant = contestants[0];

        CameraController.Instance.transitionToCMVirtualCamera(curContestant.cam); //transition camera to him to start the dialogue
    }
/*    // Update is called once per frame
    void Update()
    {
        
    }*/
}
