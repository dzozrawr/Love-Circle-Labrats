using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BakingMixingPhase : BakingMiniGameState
{
    private GameObject topPile = null, bottomPile = null, dough = null, woodenSpoon = null;

    private float progress = 0f;

    private Vector3 sugarPileStartPos, sugarPileEndPos;
    public void InitState(BakingMiniGame bmg)
    {
        bmg.bakingMiniGameCanvas.phase1UIElementsGroup.GetComponent<Animator>().SetTrigger("Hide");
        bmg.bakingMiniGameCanvas.mixingPhaseElements.SetActive(true);
        bmg.bakingMiniGameCanvas.mixingPhaseElements.GetComponent<Animation>().Play("Baking Phase Mixing");
        bmg.bakingMiniGameCanvas.bakingProgressBar.SetFill(0f);
        bmg.bakingMiniGameCanvas.bakingProgressBar.GetComponent<Animation>().Play("Progress Bar Show");

        topPile = bmg.TopPile;
        bottomPile = bmg.BottomPile;

        sugarPileEndPos= topPile.transform.position;
        sugarPileStartPos=bmg.SugarPileInitPos;

        bottomPile.SetActive(false);
        dough = bmg.dough;
        woodenSpoon = bmg.woodenSpoon;

        dough.transform.localScale = bmg.doughStartScale;
        dough.SetActive(true);
        woodenSpoon.SetActive(true);


        foreach(GameObject eggYolk in bmg.EggYolks)
        {
            eggYolk.SetActive(false);
        }
    }
    public BakingMiniGameState DoState(BakingMiniGame bmg)
    {

        if (Input.GetMouseButton(0))
        {
            topPile.transform.Rotate(Vector3.up, Time.deltaTime * 50f);
            dough.transform.Rotate(Vector3.up, Time.deltaTime * 50f);
            woodenSpoon.GetComponent<Animator>().speed = 0.6f;

            if (progress >= 1f)
            {
                progress = 1f;
            }
            else
            {
                progress += Time.deltaTime / 4;
            }
            topPile.transform.localScale = new Vector3(bmg.sugarPileEndScale.x * (1-progress), bmg.sugarPileEndScale.y * (1-progress), bmg.sugarPileEndScale.z * (1-progress));
            topPile.transform.position=new Vector3(topPile.transform.position.x,sugarPileStartPos.y+(sugarPileEndPos.y-sugarPileStartPos.y)*(1-progress),topPile.transform.position.z);
            dough.transform.localScale = new Vector3(bmg.doughStartScale.x + (bmg.doughEndScale.x - bmg.doughStartScale.x) * progress, bmg.doughStartScale.y + (bmg.doughEndScale.y - bmg.doughStartScale.y) * progress, bmg.doughStartScale.z + (bmg.doughEndScale.z - bmg.doughStartScale.z) * progress);

            bmg.bakingMiniGameCanvas.bakingProgressBar.SetFill(progress);

            if (progress >= 1f)
            {
//                Debug.Log("Mixing done");
                return bmg.bowlSwitchPhase;
            }
        }
        else
        {
            woodenSpoon.GetComponent<Animator>().speed = 0;
        }
        return this;
    }


}
