using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EggBreakingPhase : BakingMiniGameState
{
    private bool firstTimeBool=false;

    private bool isTransitioningToNextPhase=false;
    private bool shouldGoToNextPhase=false;

    public BakingMiniGameState DoState(BakingMiniGame bmg)
    {
        if(shouldGoToNextPhase){
           return bmg.flourPourPhase;
        }

        if(isTransitioningToNextPhase){
            return this;
        }
        if(!firstTimeBool){
            for (int i = 0; i < bmg.bakingMiniGameCanvas.phase1UIElements.Length; i++)
            {
              if(bmg.bakingMiniGameCanvas.phase1UIElements[i].type==BakingUIElement.BakingUIElementType.Egg){
                bmg.bakingMiniGameCanvas.phase1UIElements[i].Select(true);
                break;
              }  
            }
            firstTimeBool=true;
        }

        if(Input.GetMouseButtonDown(0)){
            Debug.Log("Eggs broken.");
            isTransitioningToNextPhase=true;
            Thread t= new Thread(GoToNextPhase);
            t.Start();
        }
        return this;
    }

    private void GoToNextPhase(){
        Thread.Sleep(1000);
        shouldGoToNextPhase=true;
    }

}
