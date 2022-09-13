using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Contestant;
public class ContestantMiniGameEOPTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ContestantScript>())
        {
            other.GetComponent<ContestantScript>().animator.SetTrigger("Idle");
            FinalEliminationManager.Instance.ContestantEndOfPathAction();
        }
    }
}
