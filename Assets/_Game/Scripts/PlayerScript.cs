using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerScript : MonoBehaviour
{
    public string conversationID = null;
    public GameObject curtain = null;
    public CinemachineVirtualCamera virutalFollowCamera = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ChoosePlayer()
    {
        Destroy(curtain); //open the curtain
        GameController.Instance.SetConversation(conversationID); //set the conversation
        //start the walking sequence and whatnot
    }
}
