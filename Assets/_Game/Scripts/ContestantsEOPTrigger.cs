using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContestantsEOPTrigger : MonoBehaviour
{
    //public PathFollower pathFollower = null;
    //public EndOfPath 
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("This is the end, hold your breath and count to 10.");
      //  pathFollower.speed = 0;
        CameraController.Instance.transitionToCMVirtualCamera(CameraController.CameraPhase.ContestantsStart);
        other.GetComponent<PlayerScript>().animator.SetTrigger("Idle");
        // other.GetComponent<PlayerScript>().virutalFollowCamera;
    }
}
