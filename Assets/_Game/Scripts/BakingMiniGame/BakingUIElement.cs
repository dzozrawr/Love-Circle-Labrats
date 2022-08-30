using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class BakingUIElement : MonoBehaviour
{
    public enum BakingUIElementType{
        Egg,Flour,Sugar
    }
    public Sprite selectedSprite = null;
    public Sprite unselectedSprite = null;

    public BakingUIElementType type;

    private Image img = null;

    private void Awake()
    {
        img = GetComponent<Image>();
        if(img==null){
            Debug.Log("img s null");
        }
    }
    public void Select(bool shouldSelect)
    {
        if (shouldSelect)img.sprite=selectedSprite; else img.sprite=unselectedSprite;
    }
}
