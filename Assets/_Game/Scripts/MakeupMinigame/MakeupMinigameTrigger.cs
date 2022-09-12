using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeupMinigameTrigger : MonoBehaviour
{
    public MakeupMiniGame makeupMiniGame=null;

    private void OnTriggerEnter(Collider other) {
        makeupMiniGame.TriggerMiniGame();
        Destroy(this);
    }
}
