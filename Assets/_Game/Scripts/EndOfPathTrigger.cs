using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfPathTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("This is the end, hold your breath and count to 10.");
        CameraController.Instance.transitionToCMVirtualCamera(other.GetComponent<PlayerScript>().virutalFollowCamera);
       // other.GetComponent<PlayerScript>().virutalFollowCamera;
    }
}
