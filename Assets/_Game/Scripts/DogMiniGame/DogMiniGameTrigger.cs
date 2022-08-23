using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DogMiniGame;

public class DogMiniGameTrigger : MonoBehaviour
{
    public GameObject dogMiniGameCanvas = null;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        dogMiniGameCanvas.SetActive(true);

        GameController.Instance.MiniGameStarted?.Invoke();
    }
}
