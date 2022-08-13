using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BeachStudioSet : StudioSet
{
    public GameObject curtainL = null, curtainR = null;

    private GameObject chosenCurtain=null;
    public override void OpenPlayerCurtain(PlayerScript player)
    {
          if (player == playerL)
        {
            chosenCurtain=curtainL;
        }
        else
        if (player == playerR)
        {
            chosenCurtain=curtainR;
        }

        chosenCurtain.transform.DOScale(Vector3.zero,0.75f).onComplete=()=>{
            GameController.Instance.CurtainOpen?.Invoke();
        };
    }
}
