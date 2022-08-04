using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakingMiniGame : MiniGame
{
    private void Awake()
    {
        models.SetActive(false);
    }
    public override void InitializeMiniGame()
    {
        models.SetActive(true);
    }
}
