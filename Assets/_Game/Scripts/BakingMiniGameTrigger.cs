using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakingMiniGameTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameController.Instance.MiniGameStarted?.Invoke();
    }
}
