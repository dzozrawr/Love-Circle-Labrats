using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EggFlourSugarPhase : BakingMiniGameState
{
    private BakingMiniGameCanvas bakingMiniGameCanvas;
    private BakingMiniGame bmg;
    private List<BrokenEgg> brokenEggs = new List<BrokenEgg>();

    private bool isEggPhaseDone = false, isFlourPhaseDone = false, isSugarPhaseDone = false;
    private bool isEggPhaseActive = false, isFlourPhaseActive = false, isSugarPhaseActive = false;

    private GameObject flourBag = null;
    private GameObject flourPile = null;

    private float flourProgress = 0f;
    private float sugarProgress = 0f;

    private Vector3 sugarPileStartScale, sugarPileEndScale, flourPileStartScale, flourPileEndScale;
    private Coroutine c = null;
    public void InitState(BakingMiniGame bmg)
    {
        bakingMiniGameCanvas = bmg.bakingMiniGameCanvas;
        this.bmg = bmg;
        /*         for (int i = 0; i < bakingMiniGameCanvas.phase1UIElements.Length; i++)
                {
                    if (bakingMiniGameCanvas.phase1UIElements[i].type == BakingUIElement.BakingUIElementType.Egg)
                    {
                        bakingMiniGameCanvas.phase1UIElements[i].Select(true);
                        break;
                    }
                } */

        for (int i = 0; i < bmg.brokenEggs.Length; i++)
        {
            brokenEggs.Add(bmg.brokenEggs[i]);
            //bmg.brokenEggs[i].gameObject.SetActive(true);
        }

        flourBag = bmg.flourBag;
        flourPile = bmg.flourPile;

        BakingMiniGameCanvas.Phase1ElementSelected.AddListener(OnPhase1ElementSelected);
    }
    public BakingMiniGameState DoState(BakingMiniGame bmg)
    {

        if (isEggPhaseDone && isFlourPhaseDone && isSugarPhaseDone)
        {
            BakingMiniGameCanvas.Phase1ElementSelected.RemoveListener(OnPhase1ElementSelected);
            return bmg.mixingPhase;
        }

        if (isEggPhaseActive && !isEggPhaseDone)
        {  //egg phase code
            if (Input.GetMouseButtonDown(0))
            {
                if (c == null)
                {
                    foreach (BrokenEgg b in brokenEggs)
                    {
                        b.BreakEgg();
                    }
                    //Debug.Log("Eggs broken.");
                    c = bmg.StartCoroutine(FinishEggPhase(1f));
                }
            }
        }

        if (isFlourPhaseActive && !isFlourPhaseDone)
        {  //flour phase code
            if (bmg.flourSpill.isSpilling)
            {
                if (flourProgress >= 1f)
                {
                    flourProgress = 1f;
                }
                else
                {
                    flourProgress += Time.deltaTime / 4;
                }

                bmg.flourPile.transform.localScale = new Vector3(flourPileStartScale.x + (flourPileEndScale.x - flourPileStartScale.x) * flourProgress, flourPileStartScale.y + (flourPileEndScale.y - flourPileStartScale.y) * flourProgress, flourPileStartScale.z + (flourPileEndScale.z - flourPileStartScale.z) * flourProgress);
                bmg.bakingMiniGameCanvas.bakingProgressBar.SetFill(flourProgress);
                if (flourProgress < 1) return this;
                else
                {
                    isFlourPhaseDone = true;
                    isFlourPhaseActive = false;

                    bakingMiniGameCanvas.bakingProgressBar.GetComponent<Animation>().Play("Progress Bar Hide");
                    flourBag.gameObject.SetActive(false);

                    bakingMiniGameCanvas.ReEnablePhase1Buttons();
                }
            }
        }

        if (isSugarPhaseActive && !isSugarPhaseDone)
        {  //sugar phase code
            if (bmg.sugarSpill.isSpilling)
            {
                if (sugarProgress >= 1f)
                {
                    sugarProgress = 1f;
                }
                else
                {
                    sugarProgress += Time.deltaTime / 4;
                }

                bmg.sugarPile.transform.localScale = new Vector3(sugarPileStartScale.x + (sugarPileEndScale.x - sugarPileStartScale.x) * sugarProgress, sugarPileStartScale.y + (sugarPileEndScale.y - sugarPileStartScale.y) * sugarProgress, sugarPileStartScale.z + (sugarPileEndScale.z - sugarPileStartScale.z) * sugarProgress);
                bmg.bakingMiniGameCanvas.bakingProgressBar.SetFill(sugarProgress);
                if (sugarProgress < 1) return this;
                else
                {
                    bakingMiniGameCanvas.bakingProgressBar.GetComponent<Animation>().Play("Progress Bar Hide");
                    bmg.sugarBox.SetActive(false);

                    isSugarPhaseDone = true;
                    isSugarPhaseActive = false;
                    bakingMiniGameCanvas.ReEnablePhase1Buttons();
                }
            }
        }



        return this;
    }

    private void OnPhase1ElementSelected()
    {
        switch (BakingMiniGameCanvas.chosenPhase1ElementType)
        {
            case BakingUIElement.BakingUIElementType.Egg:
                isEggPhaseActive = true;
                for (int i = 0; i < brokenEggs.Count; i++)
                {
                    brokenEggs[i].gameObject.SetActive(true);
                }
                break;
            case BakingUIElement.BakingUIElementType.Flour:
                isFlourPhaseActive = true;

                bakingMiniGameCanvas.bakingProgressBar.SetFill(0f);
                bakingMiniGameCanvas.bakingProgressBar.GetComponent<Animation>().Play("Progress Bar Show");
                flourBag.gameObject.SetActive(true);

                flourPileStartScale = bmg.PileScales.Pop();
                flourPileEndScale = bmg.PileScales.Pop();

                if (bmg.PileScales.Count == 0)
                {
                    bmg.TopPile = bmg.flourPile;
                    bmg.BottomPile = bmg.sugarPile;
                }

                flourPile.transform.localScale = flourPileStartScale;   //should be called differently, the scale
                flourPile.gameObject.SetActive(true);
                break;
            case BakingUIElement.BakingUIElementType.Sugar:
                isSugarPhaseActive = true;

                bakingMiniGameCanvas.bakingProgressBar.GetComponent<Animation>().Play("Progress Bar Show");
                bakingMiniGameCanvas.bakingProgressBar.SetFill(0f);

                bmg.sugarBox.SetActive(true);

                sugarPileStartScale = bmg.PileScales.Pop();
                sugarPileEndScale = bmg.PileScales.Pop();

                if (bmg.PileScales.Count == 0)
                {
                    bmg.TopPile = bmg.sugarPile;
                    bmg.BottomPile = bmg.flourPile;
                }

                bmg.sugarPile.transform.localScale = sugarPileStartScale;
                bmg.sugarPile.SetActive(true);
                break;
        }
    }
    IEnumerator FinishEggPhase(float delay)
    {
        yield return new WaitForSeconds(delay);

        for (int i = 0; i < brokenEggs.Count; i++)
        {
            brokenEggs[i].gameObject.SetActive(false);
        }

        isEggPhaseDone = true;
        isEggPhaseActive = false;
        bakingMiniGameCanvas.ReEnablePhase1Buttons();
    }

}
