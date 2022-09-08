using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DogMiniGame;

public class DancingMiniGameTrigger : MonoBehaviour
{
    public GameObject dancingMiniGameCanvas = null;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        dancingMiniGameCanvas.SetActive(true);

        GameController.Instance.MiniGameStarted?.Invoke();
    }
}
