using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakingMiniGameCanvas : MonoBehaviour
{
    public GameObject phase1UIElementsGroup = null;
    public BakingUIElement[] phase1UIElements;
    public BakingProgressBar bakingProgressBar = null;

    public GameObject phase3UIElementsGroup = null;

    private List<BakingUIElement> fruitIcons = new List<BakingUIElement>();

    private GameObject chosenFruit=null;

    public List<BakingUIElement> FruitIcons { get => fruitIcons; set => fruitIcons = value; }
    public GameObject ChosenFruit { get => chosenFruit; set => chosenFruit = value; }

    private void Awake()
    {
        foreach (BakingUIElement b in phase3UIElementsGroup.transform.GetComponentsInChildren<BakingUIElement>())
        {
            fruitIcons.Add(b);
        }    
    }

}
