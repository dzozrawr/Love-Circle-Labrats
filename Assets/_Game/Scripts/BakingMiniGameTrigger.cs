using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakingMiniGameTrigger : MonoBehaviour
{
    public GameObject bakingMiniGameCanvas = null;
    public BakingMiniGame bakingMiniGame=null;
    private void OnTriggerEnter(Collider other)
    {
        bakingMiniGame.isMiniGameStarted=true;
        bakingMiniGameCanvas.SetActive(true);
        GameController.Instance.MiniGameStarted?.Invoke();
    }
}
