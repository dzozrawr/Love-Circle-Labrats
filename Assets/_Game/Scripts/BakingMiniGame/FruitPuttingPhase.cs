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

    private GameObject hitCircleForFruit=null;
    private BakingMiniGameCanvas bakingMiniGameCanvas=null;

    private int putFruitCount=0;

    private float progress=0f;
    public void InitState(BakingMiniGame bmg)
    {
        bakingMiniGameCanvas=bmg.bakingMiniGameCanvas;
        hitCircleForFruit=bmg.hitCircleForFruit;

        bakingMiniGameCanvas.phase3UIElementsGroup.SetActive(true);
        hitCircleForFruit.SetActive(true);
        bakingMiniGameCanvas.bakingProgressBar.SetFill(0f);
        bakingMiniGameCanvas.bakingProgressBar.gameObject.SetActive(true);

        

        mainCamera = CameraController.Instance.GetComponent<Camera>();
    }
    public BakingMiniGameState DoState(BakingMiniGame bmg)
    {
        if(bakingMiniGameCanvas.ChosenFruit==null) return this;
        
        if (Input.GetMouseButton(0))
        {
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                hitObject = hit.collider.gameObject;
                if (hitObject==hitCircleForFruit){                    
                    MonoBehaviour.Instantiate(bakingMiniGameCanvas.ChosenFruit,hit.point,Quaternion.identity);
                    putFruitCount++;
                    progress=((float)putFruitCount)/(float)bakingMiniGameCanvas.MaxFruitLimit;
                    bakingMiniGameCanvas.bakingProgressBar.SetFill(progress);
                }        //hitObject is a reference to HitBox here

            }
        }
        if(progress>=1f) return bmg.topLayerPutPhase;
        
        return this;
    }


}
