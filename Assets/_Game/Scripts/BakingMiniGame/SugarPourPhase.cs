using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SugarPourPhase : BakingMiniGameState
{

    private float pourProgress = 0f;

    public void InitState(BakingMiniGame bmg)
    {
        for (int i = 0; i < bmg.bakingMiniGameCanvas.phase1UIElements.Length; i++)
        {
            if (bmg.bakingMiniGameCanvas.phase1UIElements[i].type == BakingUIElement.BakingUIElementType.Sugar)
            {
                bmg.bakingMiniGameCanvas.phase1UIElements[i].Select(true);
                break;
            }
        }

        bmg.bakingMiniGameCanvas.bakingProgressBar.gameObject.SetActive(true);
        bmg.bakingMiniGameCanvas.bakingProgressBar.SetFill(0f);

        bmg.sugarBox.SetActive(true);

        bmg.sugarPile.transform.localScale = bmg.sugarPileStartScale;
        bmg.sugarPile.SetActive(true);
    }
    public BakingMiniGameState DoState(BakingMiniGame bmg)
    {
        /*         if (!firstTimeBool)
                {
                             for (int i = 0; i < bmg.bakingMiniGameCanvas.phase1UIElements.Length; i++)
                                {
                                    if (bmg.bakingMiniGameCanvas.phase1UIElements[i].type == BakingUIElement.BakingUIElementType.Sugar)
                                    {
                                        bmg.bakingMiniGameCanvas.phase1UIElements[i].Select(true);
                                        break;
                                    }
                                }

                                bmg.bakingMiniGameCanvas.sugarProgressBar.gameObject.SetActive(true);
                                bmg.sugarBox.SetActive(true);
                                bmg.sugarPile.SetActive(true);
                    firstTimeBool = true;
                } */

        if (bmg.sugarSpill.isSpilling)
        {
            if (pourProgress >= 1f)
            {
                pourProgress = 1f;
            }
            else
            {
                pourProgress += Time.deltaTime / 4;
            }

            bmg.sugarPile.transform.localScale = new Vector3(bmg.sugarPileStartScale.x+(bmg.sugarPileEndScale.x - bmg.sugarPileStartScale.x) * pourProgress, bmg.sugarPileStartScale.y+(bmg.sugarPileEndScale.y - bmg.sugarPileStartScale.y) * pourProgress, bmg.sugarPileStartScale.z+(bmg.sugarPileEndScale.z - bmg.sugarPileStartScale.z) * pourProgress);
            bmg.bakingMiniGameCanvas.bakingProgressBar.SetFill(pourProgress);
            if (pourProgress < 1) return this;
            else
            {
               // Debug.Log("Sugar pouring done");
                bmg.sugarBox.gameObject.SetActive(false);
                return bmg.mixingPhase;    //the next phase
            }
        }
        return this;
    }


}
