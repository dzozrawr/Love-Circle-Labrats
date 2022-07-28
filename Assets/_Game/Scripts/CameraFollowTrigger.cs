using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;
using PathCreation.Utility;
using PathCreationEditor;
using PathCreation;

public class CameraFollowTrigger : MonoBehaviour
{
    public PathFollower player = null;


    private void OnTriggerEnter(Collider other)
    {
        player.speed = 3;//set the speed
      //  player.pathCreator = path;//put player on another path
       

       // Debug.Log("OnTriggerEnter");
    }
}
