using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Utility;
using PathCreationEditor;
using PathCreation.Examples;
using PathCreation;

public class EndOfPathTrigger : MonoBehaviour
{
    public PathFollower pathFollower=null;
    //public EndOfPath 
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("This is the end, hold your breath and count to 10.");
        pathFollower.speed = 0;
        CameraController.Instance.transitionToCMVirtualCamera(other.GetComponent<PlayerScript>().virutalFollowCamera);
       // other.GetComponent<PlayerScript>().virutalFollowCamera;
    }
}
