using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class BakingUIElement : MonoBehaviour
{
    public enum BakingUIElementType
    {
        Egg, Flour, Sugar, Blueberries, Cranberries, Stawberries
    }
    public Sprite selectedSprite = null;
    public Sprite unselectedSprite = null;

    public BakingUIElementType type;

    private bool isSelected=false;

    private Image img = null;

    public bool IsSelected { get => isSelected; set => isSelected = value; }

    private void Awake()
    {
        img = GetComponent<Image>();
        if (img == null)
        {
            Debug.Log("img s null");
        }
    }
    public void Select(bool shouldSelect)
    {
        if (shouldSelect) img.sprite = selectedSprite; else img.sprite = unselectedSprite;
        
        foreach (BakingUIElement b in transform.parent.GetComponentsInChildren<BakingUIElement>())
        {
            b.GetComponent<Button>().enabled=false;
        }
        isSelected=true;
        BakingMiniGameCanvas.chosenPhase1ElementType=type;
        BakingMiniGameCanvas.Phase1ElementSelected?.Invoke(); 
    }
}
