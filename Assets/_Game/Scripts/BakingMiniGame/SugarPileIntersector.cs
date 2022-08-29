using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SugarPileIntersector : MonoBehaviour
{
    /*     private void OnTriggerEnter(Collider other) {
            Debug.Log("Sugar OnTriggerEnter");
        } */
    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("BakingBowl"))
        {
            transform.position += new Vector3(0, Time.deltaTime / 4, 0);
        }
    }
}
