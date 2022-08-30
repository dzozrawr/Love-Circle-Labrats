using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BakingFruitUI : MonoBehaviour  //used to store information about the fruit model
{
    public BakingMiniGameCanvas bakingMiniGameCanvas=null;
    public GameObject fruitPrefab=null;

    public int maxFruitLimit=10;

    public void OnButtonClick(){
        bakingMiniGameCanvas.SetChosenFruit(fruitPrefab,maxFruitLimit);
        Image img;
        foreach (BakingFruitUI f in transform.parent.GetComponentsInChildren<BakingFruitUI>())
        {
            f.GetComponent<Button>().enabled=false;
            img=f.GetComponent<Image>();
            if(f!=this){
                
                img.color=new Color(img.color.r,img.color.g,img.color.b,0.5f);
            }else{
                img.sprite=f.GetComponent<BakingUIElement>().selectedSprite;
            }
        }
        
    }
}
