using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceEOETrigger : MonoBehaviour
{
     private ContestantQuestioningManager contestantQuestioningManager;

     private void Start() {
        contestantQuestioningManager=ContestantQuestioningManager.Instance;
     }
    private void OnTriggerEnter(Collider other) {
        contestantQuestioningManager.ContestantEliminatedSignal();
    }

}
