using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpaceStudioSet : StudioSet
{
    public GameObject curtainL = null, curtainR = null;

    [Range (0f,10f)]
    public float eliminationDelayBeforeGoingUp=0.5f;

    [Range (0f,20f)]
    public float eliminationGoingUpTweenDuration=1f;
    [Range (0f,50f)]
    public float eliminationGoingUpEndHeight=25f;
    private GameObject chosenCurtain = null;




    public override void OpenPlayerCurtain(PlayerScript player)
    {

        if (player == playerL)
        {
            chosenCurtain = curtainL;
        }
        else
        if (player == playerR)
        {
            chosenCurtain = curtainR;
        }

        chosenCurtain.transform.DOMoveY(chosenCurtain.transform.position.y + chosenCurtain.GetComponent<Renderer>().bounds.size.y, 0.75f).onComplete = () =>
        {
            GameController.Instance.CurtainOpen?.Invoke();
        };
    }

    public override void EliminateContestant(ContestantScript contestant)
    {
        contestant.animator.SetTrigger("Amaze"); //different animation
        /* 
                int contestantInd= contestantQuestioningManager.contestants.IndexOf(contestant);

                GameObject hole= eliminationHoleList[contestantInd]; */
        StartCoroutine(GoUpTweenAnimationAfterDelay(contestant, eliminationDelayBeforeGoingUp));
        /*         hole.transform.DOMove(hole.transform.position + hole.transform.up * hole.GetComponent<Renderer>().bounds.size.x, 0.5f).onComplete = () =>
                {

                }; */
    }

    private IEnumerator GoUpTweenAnimationAfterDelay(ContestantScript c, float delay)
    {
        yield return new WaitForSeconds(delay);
        c.animator.SetTrigger("FreeFall");
        c.transform.DOMoveY(eliminationGoingUpEndHeight, eliminationGoingUpTweenDuration).onComplete = () =>
        {
            contestantQuestioningManager.ContestantEliminatedSignal();
        };
    }
}
