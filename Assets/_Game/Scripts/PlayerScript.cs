using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using PathCreation;
using PathCreation.Examples;

public class PlayerScript : MonoBehaviour
{
    public string conversationID = null;
    public GameObject curtain = null;
    public CinemachineVirtualCamera virutalFollowCamera = null;

    [Range(0.0f, 10.0f)]  public float walkingSpeed = 2.5f;

    public Animator animator = null;
    public MiniGame miniGame = null;

    private PathFollower pathFollower = null;
    private void Awake()
    {
        pathFollower = GetComponent<PathFollower>();
    }

    public void ChoosePlayer()
    {
        Destroy(curtain); //open the curtain
        GameController.Instance.SetConversation(conversationID); //set the conversation
        pathFollower.speed = walkingSpeed;//start the walking sequence and whatnot
        animator.SetTrigger("Walk");//trigger walking animation

        miniGame.InitializeMiniGame();
        GameController.Instance.ChosenPlayer = this;
    }
}
