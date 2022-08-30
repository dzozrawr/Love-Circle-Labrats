using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BowlSwitchPhase : BakingMiniGameState
{
    private GameObject mixingBowl = null, pieDishPrefab = null;
    private Transform placeToMoveBowlTo;

    private GameObject pieDishInstance = null;
    private Vector3 bowlInitPos;

    private bool isPhaseFinished=false;
    public void InitState(BakingMiniGame bmg)
    {
        mixingBowl = bmg.mixingBowl;
        pieDishPrefab = bmg.pieDishPrefab;
        placeToMoveBowlTo = bmg.bowlMovedPlace;

        bmg.bakingMiniGameCanvas.bakingProgressBar.gameObject.SetActive(false);

        bowlInitPos = mixingBowl.transform.position;

        mixingBowl.transform.DOMove(placeToMoveBowlTo.position, 1.5f).OnComplete(() =>
        {
            mixingBowl.SetActive(false);
            pieDishInstance = MonoBehaviour.Instantiate(pieDishPrefab, placeToMoveBowlTo.position, Quaternion.identity);
            pieDishInstance.transform.SetParent(bmg.models.transform);
            pieDishInstance.transform.DOMove(bowlInitPos,1.5f).OnComplete(()=>{
                isPhaseFinished=true;
            });
        });
    }
    public BakingMiniGameState DoState(BakingMiniGame bmg)
    {
        if(isPhaseFinished){
            return bmg.fruitPhase;
        }
        return this;
    }


}
