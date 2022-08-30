using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakingMiniGameCanvas : MonoBehaviour
{
    public GameObject phase1UIElementsGroup = null;
    public BakingUIElement[] phase1UIElements;
    public BakingProgressBar bakingProgressBar = null;

    public GameObject phase3UIElementsGroup = null;

    public GameObject pieCuttingUIElementsGroup=null;

    private List<BakingUIElement> fruitIcons = new List<BakingUIElement>();

    private GameObject chosenFruit=null;
    private int maxFruitLimit;

    private int chosenPatternNumber=-1;

    public List<BakingUIElement> FruitIcons { get => fruitIcons; set => fruitIcons = value; }
    public GameObject ChosenFruit { get => chosenFruit; set => chosenFruit = value; }
    public int MaxFruitLimit { get => maxFruitLimit; set => maxFruitLimit = value; }
    public int ChosenPatternNumber { get => chosenPatternNumber; set => chosenPatternNumber = value; }

    private void Awake()
    {
        foreach (BakingUIElement b in phase3UIElementsGroup.transform.GetComponentsInChildren<BakingUIElement>())
        {
            fruitIcons.Add(b);
        }    
    }

    public void SetChosenFruit(GameObject fruit, int maxLimit){
        chosenFruit=fruit;
        maxFruitLimit=maxLimit;
    }

}
