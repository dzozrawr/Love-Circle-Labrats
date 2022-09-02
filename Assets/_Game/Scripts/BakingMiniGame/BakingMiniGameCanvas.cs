using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using BakingMinigameFruit;
using UnityEngine.UI;

public class BakingMiniGameCanvas : MonoBehaviour
{
    public GameObject phase1UIElementsGroup = null;
    public BakingUIElement[] phase1UIElements;
    public BakingProgressBar bakingProgressBar = null;

    public GameObject mixingPhaseElements = null;

    public GameObject phase3UIElementsGroup = null;

    public GameObject pieCuttingUIElementsGroup = null;

    public static UnityEvent Phase1ElementSelected = new UnityEvent();

    public static BakingUIElement.BakingUIElementType chosenPhase1ElementType;

    private List<BakingUIElement> fruitIcons = new List<BakingUIElement>();

    private GameObject chosenFruit = null;
    private FruitColor chosenFruitColor;
    private int maxFruitLimit;

    private int chosenPatternNumber = -1;

    public List<BakingUIElement> FruitIcons { get => fruitIcons; set => fruitIcons = value; }
    public GameObject ChosenFruit { get => chosenFruit; set => chosenFruit = value; }
    public int MaxFruitLimit { get => maxFruitLimit; set => maxFruitLimit = value; }
    public int ChosenPatternNumber { get => chosenPatternNumber; set => chosenPatternNumber = value; }
    public FruitColor ChosenFruitColor { get => chosenFruitColor; set => chosenFruitColor = value; }

    private void Awake()
    {
        foreach (BakingUIElement b in phase3UIElementsGroup.transform.GetComponentsInChildren<BakingUIElement>())
        {
            fruitIcons.Add(b);
        }
    }

    public void SetChosenFruit(GameObject fruit, int maxLimit, FruitColor c)
    {
        chosenFruit = fruit;
        maxFruitLimit = maxLimit;
        chosenFruitColor = c;
    }

    public void ReEnablePhase1Buttons()
    {//reenables buttons that were not yet pressed
        foreach (BakingUIElement b in phase1UIElementsGroup.GetComponentsInChildren<BakingUIElement>())
        {
            if(!b.IsSelected){
                b.GetComponent<Button>().enabled=true;
            }
        }

    }

}
