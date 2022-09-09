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

    public GameObject phase1TapMsgGO = null, phase1TapAndHoldMsgGO = null;
    public BakingProgressBar bakingProgressBar = null;

    public GameObject mixingPhaseElements = null;

    public GameObject mixingPhaseTapAndHoldMsgGO = null;

    public GameObject phase3UIElementsGroup = null;

    public GameObject fruitPuttingPhaseTapMsgGO = null;

    public GameObject pieCuttingUIElementsGroup = null;

    public static UnityEvent Phase1ElementSelected = new UnityEvent();

    public static BakingUIElement.BakingUIElementType chosenPhase1ElementType;

    private List<BakingUIElement> fruitIcons = new List<BakingUIElement>();

    private FruitType chosenFruitType;
    private FruitColor chosenFruitColor;
    private int maxFruitLimit;

    private int chosenPatternNumber = -1;
    [HideInInspector]
    public UnityEvent FruitChosen;

    public List<BakingUIElement> FruitIcons { get => fruitIcons; set => fruitIcons = value; }
    public FruitType ChosenFruitType { get => chosenFruitType; set => chosenFruitType = value; }
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

    private void Start()
    {
        Canvas canvas = GetComponent<Canvas>();
        if (canvas.worldCamera == null)
        {
         
            GameObject go = GameObject.FindGameObjectWithTag("CameraUI");
            if (go != null)
            {
                
                canvas.worldCamera = go.GetComponent<Camera>();
            }
        }
    }

    public void SetChosenFruit(FruitType fruitType, FruitColor c)
    {
        chosenFruitType = fruitType;
        chosenFruitColor = c;
        FruitChosen?.Invoke();
    }

    public void ReEnablePhase1Buttons()
    {//reenables buttons that were not yet pressed
        foreach (BakingUIElement b in phase1UIElementsGroup.GetComponentsInChildren<BakingUIElement>())
        {
            if (!b.IsSelected)
            {
                b.GetComponent<Button>().enabled = true;
            }
        }

    }

}
