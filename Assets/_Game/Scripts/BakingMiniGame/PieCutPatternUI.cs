using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieCutPatternUI : MonoBehaviour
{
    public BakingMiniGameCanvas bakingMiniGameCanvas = null;

    public void OnButtonClick()
    {
        bakingMiniGameCanvas.ChosenPatternNumber=transform.GetSiblingIndex();
        Image img;
        foreach (PieCutPatternUI p in transform.parent.GetComponentsInChildren<PieCutPatternUI>())
        {
            p.GetComponent<Button>().enabled = false;
            img = p.GetComponent<Image>();
            if (p != this)
            {
                img.color = new Color(img.color.r, img.color.g, img.color.b, 0.5f);
            }
            else
            {
                img.sprite = p.GetComponent<BakingUIElement>().selectedSprite;
            }
        }

    }
}
