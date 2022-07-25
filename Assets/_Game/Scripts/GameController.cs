using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameController : MonoBehaviour
{
    private static GameController instance=null;
    public static GameController Instance { get => instance; }

    public delegate void ConversationChangeHandler(string conversationName);
    public event ConversationChangeHandler OnConversationChanged;

    //public 

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

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
            SetConversation("Amelia - Baking");
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SetConversation("Olivia - Dogs");
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
