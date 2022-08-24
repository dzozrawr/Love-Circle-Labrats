using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class GameController : MonoBehaviour //all of the events are in this class
{
    private static GameController instance = null;
    public static GameController Instance { get => instance; }


    public delegate void ConversationChangeHandler(string conversationName);
    public event ConversationChangeHandler OnConversationChanged;

    private PlayerScript chosenPlayer = null;
    public PlayerScript ChosenPlayer { get => chosenPlayer; set => chosenPlayer = value; }

    [HideInInspector]
    public UnityEvent ContestantsEliminated, CurtainOpen, MiniGameStarted;

    public StudioSet[] studioSetList = null;
    public StudioSet studioSet = null;

    private int studioSetIndex = 0;


    //public PlayerScript leftPlayer = null, rightPlayer=null;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

#if UNITY_EDITOR
    private void Start()
    {
        for (int i = 0; i < studioSetList.Length; i++)
        {
            if(studioSetList[i].gameObject.activeSelf){
                studioSet=studioSetList[i];
                break;
            } 
        }
    }
#endif

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR


        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartContestantsPhase();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            studioSet.gameObject.SetActive(false);
            studioSetIndex = (studioSetIndex + studioSetList.Length - 1) % studioSetList.Length;
            studioSet = studioSetList[studioSetIndex];
            studioSet.gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            studioSet.gameObject.SetActive(false);
            studioSetIndex = (studioSetIndex + 1) % studioSetList.Length;
            studioSet = studioSetList[studioSetIndex];
            studioSet.gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            CameraController.Instance.transitionToCMVirtualCamera(CameraController.CameraPhase.DogMiniGame);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            CameraController.Instance.transitionToCMVirtualCamera(CameraController.CameraPhase.BakingMiniGame);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            CameraController.Instance.transitionToCMVirtualCamera(CameraController.CameraPhase.ContestantsElimination);
        }

#endif



    }

    public void SetConversation(string conversationName)
    {
        OnConversationChanged?.Invoke(conversationName);
    }

    public void StartPlayerPicking()
    {
        CameraController.Instance.transitionToCMVirtualCamera(CameraController.CameraPhase.PlayerPicking);
        //  CameraController.Instance.playerPickingCam.Priority = CameraController.Instance.introCam.Priority + 1;
    }

    public void StartContestantsPhase()
    {
        CameraController.Instance.transitionToCMVirtualCamera(CameraController.CameraPhase.ContestantsStart);
        //CameraController.Instance.contestantsCam.Priority = CameraController.Instance.playerPickingCam.Priority + 1;
    }
}
