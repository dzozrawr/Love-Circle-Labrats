using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakingMiniGameTrigger : MonoBehaviour
{
    public GameObject bakingMiniGameCanvas = null;
    private void OnTriggerEnter(Collider other)
    {
        bakingMiniGameCanvas.SetActive(true);
        GameController.Instance.MiniGameStarted?.Invoke();
    }
}
