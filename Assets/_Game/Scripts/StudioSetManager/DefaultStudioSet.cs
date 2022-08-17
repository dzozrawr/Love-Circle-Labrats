using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DefaultStudioSet : StudioSet
{
    public GameObject curtainL = null, curtainR = null;

    public GameObject[] eliminationHoleList = null;

    [Range(0f, 5f)]
    public float eliminationHoleTweenDuration = 0.5f;
    [Range(0f, 5f)]
    public float eliminationDelayBeforeDroppingDown = 0.5f;

    [Range(0f, 5f)]
    public float eliminationDroppingDownTweenDuration = 0.25f;
    [Range(-10f, 0)]
    public float eliminationDroppingDownEndHeight = -5f;
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
        contestant.animator.SetTrigger("Amaze");

        int contestantInd = contestantQuestioningManager.contestants.IndexOf(contestant);

        GameObject hole = eliminationHoleList[contestantInd];
        hole.transform.DOMove(hole.transform.position + hole.transform.up * hole.GetComponent<Renderer>().bounds.size.x, eliminationHoleTweenDuration).onComplete = () =>
        {
            StartCoroutine(DropTweenAnimationAfterDelay(contestant, eliminationDelayBeforeDroppingDown));
        };

    }

    private IEnumerator DropTweenAnimationAfterDelay(ContestantScript c, float delay)
    {
        yield return new WaitForSeconds(delay);
        c.animator.SetTrigger("FreeFall");
        yield return new WaitForSeconds(delay);
        
        c.transform.DOMoveY(eliminationDroppingDownEndHeight, eliminationDroppingDownTweenDuration).onComplete = () =>
        {
            contestantQuestioningManager.ContestantEliminatedSignal();
        };
    }
}
