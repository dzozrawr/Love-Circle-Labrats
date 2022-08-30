using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FlourPourPhase : BakingMiniGameState
{
    private bool firstTimeBool = false;

    private bool isTransitioningToNextPhase = false;
    private bool shouldGoToNextPhase = false;

    private float pourProgress = 0f;

    public void InitState(BakingMiniGame bmg)
    {
        for (int i = 0; i < bmg.bakingMiniGameCanvas.phase1UIElements.Length; i++)
        {
            if (bmg.bakingMiniGameCanvas.phase1UIElements[i].type == BakingUIElement.BakingUIElementType.Flour)
            {
                bmg.bakingMiniGameCanvas.phase1UIElements[i].Select(true);
                break;
            }
        }

        bmg.bakingMiniGameCanvas.bakingProgressBar.SetFill(0f);
        bmg.bakingMiniGameCanvas.bakingProgressBar.gameObject.SetActive(true);
        bmg.flourBag.gameObject.SetActive(true);
        bmg.flourPile.transform.localScale = bmg.flourPileStartScale;
        bmg.flourPile.gameObject.SetActive(true);
    }
    public BakingMiniGameState DoState(BakingMiniGame bmg)
    {
        if (shouldGoToNextPhase)
        {
            return bmg.sugarPourPhase;
        }

        if (isTransitioningToNextPhase)
        {
            return this;
        }

        if (!firstTimeBool)
        {

            firstTimeBool = true;
        }

        if (bmg.flourSpill.isSpilling)
        {
            if (pourProgress >= 1f)
            {
                pourProgress = 1f;
            }
            else
            {
                pourProgress += Time.deltaTime / 4;
            }

            bmg.flourPile.transform.localScale = new Vector3((bmg.flourPileEndScale.x - bmg.flourPileStartScale.x) * pourProgress, (bmg.flourPileEndScale.y - bmg.flourPileStartScale.y) * pourProgress, (bmg.flourPileEndScale.z - bmg.flourPileStartScale.z) * pourProgress);
            bmg.bakingMiniGameCanvas.bakingProgressBar.SetFill(pourProgress);
            if (pourProgress < 1) return this;
            else
            {
                Debug.Log("Flour pouring done");
                bmg.flourBag.gameObject.SetActive(false);
                return bmg.sugarPourPhase;
            }
        }

        /*         if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Flour poured.");
                    isTransitioningToNextPhase = true;
                    Thread t = new Thread(GoToNextPhase);
                    t.Start();
                } */
        return this;
    }


    private void GoToNextPhase()
    {
        Thread.Sleep(1000);
        shouldGoToNextPhase = true;
    }
}
