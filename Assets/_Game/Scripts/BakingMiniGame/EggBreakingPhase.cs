using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EggBreakingPhase : BakingMiniGameState
{
    private bool firstTimeBool = false;

    private bool isTransitioningToNextPhase = false;
    private bool shouldGoToNextPhase = false;

    private List<BrokenEgg> brokenEggs = new List<BrokenEgg>();



    public void InitState(BakingMiniGame bmg)
    {
        for (int i = 0; i < bmg.bakingMiniGameCanvas.phase1UIElements.Length; i++)
        {
            if (bmg.bakingMiniGameCanvas.phase1UIElements[i].type == BakingUIElement.BakingUIElementType.Egg)
            {
                bmg.bakingMiniGameCanvas.phase1UIElements[i].Select(true);
                break;
            }
        }

        for (int i = 0; i < bmg.brokenEggs.Length; i++)
        {
            brokenEggs.Add(bmg.brokenEggs[i]);
            bmg.brokenEggs[i].gameObject.SetActive(true);
        }

    }


    public BakingMiniGameState DoState(BakingMiniGame bmg)
    {
        if (shouldGoToNextPhase)
        {
            foreach (BrokenEgg b in brokenEggs)
            {
                b.gameObject.SetActive(false);
            }
            return bmg.flourPourPhase;
        }

        if (isTransitioningToNextPhase)
        {
            return this;
        }
        if (!firstTimeBool)
        {
            /*             for (int i = 0; i < bmg.bakingMiniGameCanvas.phase1UIElements.Length; i++)
                        {
                            if (bmg.bakingMiniGameCanvas.phase1UIElements[i].type == BakingUIElement.BakingUIElementType.Egg)
                            {
                                bmg.bakingMiniGameCanvas.phase1UIElements[i].Select(true);
                                break;
                            }
                        } */
            firstTimeBool = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            foreach (BrokenEgg b in brokenEggs)
            {
                b.BreakEgg();
            }
            //Debug.Log("Eggs broken.");
            isTransitioningToNextPhase = true;
            Thread t = new Thread(GoToNextPhase);
            t.Start();
        }
        return this;
    }


    private void GoToNextPhase()
    {
        Thread.Sleep(1000);

        shouldGoToNextPhase = true;
    }

}
