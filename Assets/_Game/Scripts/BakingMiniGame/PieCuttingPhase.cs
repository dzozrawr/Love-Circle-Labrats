using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PieCuttingPhase : BakingMiniGameState
{
    private BakingMiniGameCanvas bakingMiniGameCanvas;
    private PieDish pieDish;

    private GameObject pieCutter = null;
    private GameObject pieTopLayerCut = null;

    private bool isPhaseDone = false;

    private Vector3 pieCutterInitPos;

    private bool onceBool = false;
    public void InitState(BakingMiniGame bmg)
    {
        bakingMiniGameCanvas = bmg.bakingMiniGameCanvas;
        pieDish = bmg.PieDish;

        // bmg.bakingMiniGameCanvas.pieCuttingUIElementsGroup.SetActive(true);

        CameraController.Instance.transitionToCMVirtualCamera(bmg.topDownBakingCamera);
        CheckForCameraBlending.onCameraBlendFinished += () => bmg.bakingMiniGameCanvas.pieCuttingUIElementsGroup.SetActive(true);
        //bmg.topDownBakingCamera.OnTargetObjectWarped()
    }
    public BakingMiniGameState DoState(BakingMiniGame bmg)
    {
        if (isPhaseDone)
        {
            bmg.bakingMiniGameCanvas.pieCuttingUIElementsGroup.SetActive(false);
            return bmg.bakingPhase;
        }
        if (bakingMiniGameCanvas.ChosenPatternNumber == -1) return this;

        if (!onceBool)
        {
            pieCutter = pieDish.pieCutters[bakingMiniGameCanvas.ChosenPatternNumber];
            pieTopLayerCut = pieDish.pieTopLayers[bakingMiniGameCanvas.ChosenPatternNumber];

            pieCutterInitPos = pieCutter.transform.position;
            pieCutter.transform.position = pieDish.pieCutterAbovePos.position;
            pieCutter.gameObject.SetActive(true);

            pieCutter.transform.DOMove(pieCutterInitPos, 1f).OnComplete(() =>
            {
                bmg.StartCoroutine(MoveCutterUpWDelay(0.5f));
            });
            onceBool = true;
        }


        return this;
    }

    IEnumerator MoveCutterUpWDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        pieDish.topLayer.SetActive(false);
        pieTopLayerCut.SetActive(true);

        pieCutter.transform.DOMove(pieDish.pieCutterAbovePos.position, 1f).OnComplete(() =>
        {
            isPhaseDone = true;
            pieCutter.SetActive(false);
        });
    }

}
