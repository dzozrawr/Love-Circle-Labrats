using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TopLayerPutPhase : BakingMiniGameState
{
    private PieDish pieDish;
    private Vector3 pieDishInitPos;

    private bool isPhaseFinished=false;
    public void InitState(BakingMiniGame bmg)
    {
        bmg.bakingMiniGameCanvas.phase3UIElementsGroup.GetComponent<Animator>().SetTrigger("Hide");
        bmg.bakingMiniGameCanvas.bakingProgressBar.GetComponent<Animation>().Play("Progress Bar Hide");
        /*for (int i = 0; i < bmg.bakingMiniGameCanvas.transform.childCount; i++)
        {
            bmg.bakingMiniGameCanvas.transform.GetChild(i).gameObject.SetActive(false);
        }*/
        pieDish = bmg.PieDish;
        pieDishInitPos=pieDish.topLayer.transform.position;
        pieDish.topLayer.SetActive(true);
        pieDish.topLayer.transform.position=pieDish.topLayerAbovePos.position;
        pieDish.topLayer.transform.DOMove(pieDishInitPos,1f).OnComplete(()=>{
            isPhaseFinished=true;
        });
    }
    public BakingMiniGameState DoState(BakingMiniGame bmg)
    {
        if(isPhaseFinished){
            return bmg.pieCuttingPhase;    //return next phase
        }
        return this;
    }


}
