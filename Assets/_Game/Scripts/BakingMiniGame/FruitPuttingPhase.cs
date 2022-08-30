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
    public void InitState(BakingMiniGame bmg)
    {
        bakingMiniGameCanvas=bmg.bakingMiniGameCanvas;
        hitCircleForFruit=bmg.hitCircleForFruit;

        bakingMiniGameCanvas.phase3UIElementsGroup.SetActive(true);
        hitCircleForFruit.SetActive(true);

        

        mainCamera = CameraController.Instance.GetComponent<Camera>();
    }
    public BakingMiniGameState DoState(BakingMiniGame bmg)
    {
        if (Input.GetMouseButton(0))
        {
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                hitObject = hit.collider.gameObject;
                if (hitObject==hitCircleForFruit){
                    MonoBehaviour.Instantiate(bakingMiniGameCanvas.ChosenFruit,hit.point,Quaternion.identity);
                }        //hitObject is a reference to HitBox here

            }
        }
        return this;
    }


}
