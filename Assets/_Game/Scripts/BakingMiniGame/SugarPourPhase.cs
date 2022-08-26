using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SugarPourPhase : BakingMiniGameState
{
    private bool firstTimeBool = false;
    public BakingMiniGameState DoState(BakingMiniGame bmg)
    {
        if (!firstTimeBool)
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
        }

        if (bmg.sugarSpill.isSpilling)
        {
            bmg.sugarPile.transform.localScale = new Vector3(bmg.sugarPile.transform.localScale.x > 1 ? 1 : (bmg.sugarPile.transform.localScale.x + Time.deltaTime / 4), bmg.sugarPile.transform.localScale.y > 1 ? 1 : (bmg.sugarPile.transform.localScale.y + Time.deltaTime / 4), bmg.sugarPile.transform.localScale.z > 1 ? 1 : (bmg.sugarPile.transform.localScale.z + Time.deltaTime / 4));
            bmg.bakingMiniGameCanvas.sugarProgressBar.SetFill(bmg.sugarPile.transform.localScale.x);
            if (bmg.sugarPile.transform.localScale.x < 1) return this;
            else
            {
                Debug.Log("Sugar pouring done");
                return this;
            }
        }
        return this;
    }
}
