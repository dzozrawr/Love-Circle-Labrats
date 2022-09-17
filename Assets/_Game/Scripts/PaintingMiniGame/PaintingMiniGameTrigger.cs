using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingMiniGameTrigger : MonoBehaviour
{
    public PaintingMiniGame paintingMiniGame = null;
    private void OnTriggerEnter(Collider other)
    {
        paintingMiniGame.TriggerMiniGame();
        Destroy(this);
    }
}
