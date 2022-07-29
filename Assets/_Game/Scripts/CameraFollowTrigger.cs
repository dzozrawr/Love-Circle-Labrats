using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;
using PathCreation;

public class CameraFollowTrigger : MonoBehaviour
{
    public PlayerScript player = null;

    private PathFollower pathFollower = null;
  //  public PathFollower player = null;

    private void Awake()
    {
        pathFollower = player.GetComponent<PathFollower>();
    }


    private void OnTriggerEnter(Collider other)
    {
        pathFollower.speed = 3;//set the speed
        player.animator.SetTrigger("Walk");
      //  player.pathCreator = path;//put player on another path


        // Debug.Log("OnTriggerEnter");
    }
}
