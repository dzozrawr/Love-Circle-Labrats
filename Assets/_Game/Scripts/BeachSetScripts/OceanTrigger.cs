using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanTrigger : MonoBehaviour
{
    private ContestantQuestioningManager contestantQuestioningManager;
    private void Start() {
        contestantQuestioningManager=ContestantQuestioningManager.Instance;
    }
    private void OnTriggerEnter(Collider other) {
        //add splash particle and whatnot, sound maybe
        contestantQuestioningManager.ContestantEliminatedSignal();
        other.gameObject.AddComponent<DestroyAfterDelay>();
    }
}
