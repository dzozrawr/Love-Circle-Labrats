using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using BakingMinigameFruit;

public class BakingPhase : BakingMiniGameState
{
    private PieDish pieDish;
    private Transform placeToMoveBowlTo;
    private Vector3 pieDishInitPos;

    private bool isPhaseFinished = false;

    private GameObject[] coloredBakedTopLayer;
    public void InitState(BakingMiniGame bmg)
    {
        pieDish = bmg.PieDish;
        placeToMoveBowlTo = bmg.bowlMovedPlace;

        pieDishInitPos = pieDish.transform.position;



        pieDish.transform.DOMove(placeToMoveBowlTo.position, 1.5f).OnComplete(() =>
        {
            pieDish.unbakedBody.SetActive(false);//disable unbaked base
            FruitManager.DestroyAllInstances();//destroy all fruit
            pieDish.pieTopLayers[bmg.bakingMiniGameCanvas.ChosenPatternNumber].SetActive(false);//disable the correct patterned top layer


            pieDish.bakedBody.SetActive(true);//enable baked base

            switch (bmg.bakingMiniGameCanvas.ChosenFruitColor)
            {//enable the correct baked top layer
                case FruitColor.Red:
                    coloredBakedTopLayer = pieDish.pieBakedRedTopLayers;
                    break;
                case FruitColor.Blue:
                    coloredBakedTopLayer = pieDish.pieBakedBlueTopLayers;
                    break;
            }

            coloredBakedTopLayer[bmg.bakingMiniGameCanvas.ChosenPatternNumber].SetActive(true);//enable the correct pattern

            pieDish.goodBakedParticles.gameObject.SetActive(true);
            pieDish.goodBakedParticles.Play();

            pieDish.transform.DOMove(pieDishInitPos, 1.5f).OnComplete(() =>
            {
                isPhaseFinished = true;
            });
        });
    }
    public BakingMiniGameState DoState(BakingMiniGame bmg)
    {
        if (isPhaseFinished)
        {
            return this;//next phase or end
        }
        return this;
    }


}
