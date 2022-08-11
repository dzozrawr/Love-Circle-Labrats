using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogMiniGameM : MiniGame
{

    private void Awake()
    {
        models.SetActive(false);
    }
    public override void InitializeMiniGame()
    {
        models.SetActive(true);
        canvas.gameObject.SetActive(false);
        miniGameCam.gameObject.SetActive(true); //this will be the same
    }
}