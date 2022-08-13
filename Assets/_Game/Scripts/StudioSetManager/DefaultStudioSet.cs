using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DefaultStudioSet : StudioSet
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

        chosenCurtain.transform.DOMoveY(chosenCurtain.transform.position.y+chosenCurtain.GetComponent<Renderer>().bounds.size.y,0.75f).onComplete=()=>{
            GameController.Instance.CurtainOpen?.Invoke();
        };
    }
}
