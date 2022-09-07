using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using PathCreation;
using PathCreation.Examples;
using UnityEngine.Events;

public class PlayerScript : MonoBehaviour
{
    public string conversationID = null;
    public CinemachineVirtualCamera virutalFollowCamera = null;

    [Range(0.0f, 10.0f)] public float walkingSpeed = 2.5f;

    public Animator animator = null;
    public MiniGame miniGame = null;

    public GameObject playerModel = null;

    private PathFollower pathFollower = null;
    private GameController gameController = null;

    public MiniGame miniGamePrefab=null;
    public PrefabReferenceHolder selfReferencePrefabHolder=null;
    private void Awake()
    {
        pathFollower = GetComponent<PathFollower>();
    }

    private void Start()
    {
        gameController = GameController.Instance;

    }


    public void ChoosePlayer()
    {

        //gameController.AddListenerForMiniGameEnd(this);
        gameController.studioSet.OpenPlayerCurtain(this);//based on the studio open this curtain (or smth else) in this or that way - studioSet.OpenCurtain(this)

        gameController.CurtainOpen.AddListener(ActionAfterCurtainOpen);
    }

    private void ActionAfterCurtainOpen()
    {
        gameController.SetConversation(conversationID); //set the conversation
        pathFollower.speed = walkingSpeed;//start the walking sequence and whatnot
        animator.SetTrigger("Walk");//trigger walking animation

        gameController.ChoosePlayer(this);

        miniGame.InitializeMiniGame();


        gameController.CurtainOpen.RemoveListener(ActionAfterCurtainOpen);
    }



}
