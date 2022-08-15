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
        Debug.Log("Ocean trigger");
        contestantQuestioningManager.ContestantEliminatedSignal();
    }
}
