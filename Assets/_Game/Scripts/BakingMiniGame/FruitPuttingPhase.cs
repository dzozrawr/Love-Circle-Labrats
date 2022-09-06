using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitPuttingPhase : BakingMiniGameState
{
    #region Raycast variables
    private Ray ray;
    private RaycastHit hit;
    private GameObject hitObject;
    #endregion

    private Camera mainCamera = null;

    private GameObject hitCircleForFruit = null;
    private BakingMiniGameCanvas bakingMiniGameCanvas = null;

    private int putFruitCount = 0;

    private float progress = 0f;

    private bool wasFruitChosen = false;

    private GameObject fruitBowl = null;

    private bool onceBool = false;

    private SpillingBowlScript spillingBowlScript = null;

    private BakingMiniGame bmg = null;
    public void InitState(BakingMiniGame bmg)
    {
        this.bmg = bmg;

        bakingMiniGameCanvas = bmg.bakingMiniGameCanvas;
        hitCircleForFruit = bmg.hitCircleForFruit;

        bakingMiniGameCanvas.phase3UIElementsGroup.SetActive(true);
        bakingMiniGameCanvas.mixingPhaseElements.SetActive(false);
        hitCircleForFruit.SetActive(true);
        bakingMiniGameCanvas.bakingProgressBar.SetFill(0f);
        bakingMiniGameCanvas.bakingProgressBar.GetComponent<Animation>().Play("Progress Bar Show");

        bakingMiniGameCanvas.FruitChosen.AddListener(OnFruitChosen);


        mainCamera = CameraController.Instance.GetComponent<Camera>();
    }
    public BakingMiniGameState DoState(BakingMiniGame bmg)
    {
        if (!wasFruitChosen) return this;   //do an event instead of this
        if (!onceBool)
        {

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("if (Input.GetMouseButton(0))");
                fruitBowl =MonoBehaviour.Instantiate(bmg.GetFruitBowl(bakingMiniGameCanvas.ChosenFruitType));


                fruitBowl.transform.position = bmg.placeForFruitBowl.position;
                fruitBowl.transform.rotation = bmg.placeForFruitBowl.rotation;


                /*              spillingBowlScript = fruitBowl.transform.GetChild(0).GetComponent<SpillingBowlScript>();
                              spillingBowlScript.
                                  BowlDestroyed.
                                  AddListener(OnBowlDestroyed);*/
                bmg.StartCoroutine(FinishPhaseAfterDelay(3.5f));


                onceBool = true;
            }

            
            
        }
        if(progress>=1f) return bmg.topLayerPutPhase;

        return this;
    }

    private void OnFruitChosen()
    {
        wasFruitChosen = true;
        bakingMiniGameCanvas.FruitChosen.RemoveListener(OnFruitChosen);
    }

    private void OnBowlDestroyed()
    {
        bmg.StartCoroutine(FinishPhaseAfterDelay(0.5f));
        if (spillingBowlScript != null) spillingBowlScript.BowlDestroyed.RemoveListener(OnBowlDestroyed);

    }

    IEnumerator FinishPhaseAfterDelay(float delay)  //kind of hard coded
    {
        yield return new WaitForSeconds(delay);
        progress = 1f;
    }


}
