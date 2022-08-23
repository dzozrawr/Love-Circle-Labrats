using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanTrigger : MonoBehaviour
{
    private ContestantQuestioningManager contestantQuestioningManager;
    private void Start()
    {
        contestantQuestioningManager = ContestantQuestioningManager.Instance;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Contestant"))
        {
            //add splash particle and whatnot, sound maybe
            contestantQuestioningManager.ContestantEliminatedSignal();
            ContestantScript contestantScript= other.transform.GetComponentInParent<ContestantScript>();
            Destroy(contestantScript.gameObject,1f);
        }
    }
}
