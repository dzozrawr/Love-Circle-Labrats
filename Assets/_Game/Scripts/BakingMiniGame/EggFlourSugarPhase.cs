using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggFlourSugarPhase : BakingMiniGameState
{
    private BakingMiniGameCanvas bakingMiniGameCanvas;

    private List<BrokenEgg> brokenEggs = new List<BrokenEgg>();

    private bool isEggPhaseDone = false, isFlourPhaseDone = false, isSugarPhaseDone = false;
    public void InitState(BakingMiniGame bmg)
    {
        bakingMiniGameCanvas = bmg.bakingMiniGameCanvas;
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
        BakingMiniGameCanvas.Phase1ElementSelected.AddListener(OnPhase1ElementSelected);
    }
    public BakingMiniGameState DoState(BakingMiniGame bmg)
    {

        return this;
    }

    private void OnPhase1ElementSelected()
    {
        switch(BakingMiniGameCanvas.chosenPhase1ElementType){
            case BakingUIElement.BakingUIElementType.Egg:
            break;
                        case BakingUIElement.BakingUIElementType.Flour:
            break;
                        case BakingUIElement.BakingUIElementType.Sugar:
            break;
        }
    }


}
