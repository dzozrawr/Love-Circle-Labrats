using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;
using PathCreation;
using PathCreation.Examples;

public class GameController : MonoBehaviour //all of the events are in this class
{
    private static GameController instance = null;
    public static GameController Instance { get => instance; }

    private static GameObject unchosenPlayerPrefab = null;


    public delegate void ConversationChangeHandler(string conversationName);
    public event ConversationChangeHandler OnConversationChanged;

    private PlayerScript chosenPlayer = null;
    public PlayerScript ChosenPlayer { get => chosenPlayer; set => chosenPlayer = value; }
    public static int CoinAmount { get => coinAmount; }

    [HideInInspector]
    public UnityEvent ContestantsEliminated, CurtainOpen, MiniGameStarted, CoinAmountUpdated;

    public StudioSet[] studioSetList = null;
    public StudioSet studioSet = null;
    public GameObject host = null;
    public Transform placeForHostBeforeMiniGame = null;

    public AudioClip afterMiniGameAudioClip = null;
    [Range(0, 1f)]
    public float afterMiniGameAudioClipVolume = 1f;

    public PlayerScript playerL = null;
    public PlayerScript playerR = null;

    public Transform startingPlayerRTransform = null;

    public PathCreator playerPathR = null;

    private StudioSet selectedStudioSetInMenu = null;
    private int studioSetIndex = 0;
    private static int coinAmount;




    //public PlayerScript leftPlayer = null, rightPlayer=null;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        selectedStudioSetInMenu = studioSet;
        coinAmount = 0;


    }

    private void Start()
    {

#if UNITY_EDITOR
        for (int i = 0; i < studioSetList.Length; i++)
        {
            if (studioSetList[i].gameObject.activeSelf)
            {
                studioSet = studioSetList[i];
                break;
            }
        }
#endif
        if (unchosenPlayerPrefab != null)
        {
            AddUnchosenPlayer();
        }
        else
        {
            Debug.Log("unchosenPlayer=null");
        }
    }



    public void AddUnchosenPlayer()
    {
        Debug.Log("AddUnchosenPlayer()");
        GameObject go = Instantiate(unchosenPlayerPrefab, startingPlayerRTransform.position, Quaternion.identity);
        playerR = go.GetComponent<PlayerScript>();

        playerR.miniGame = Instantiate(playerR.miniGamePrefab);

        //= go.GetComponent<MiniGame>();
        playerR.GetComponent<PathFollower>().pathCreator = playerPathR;
    }

    public void AddListenerForMiniGameEnd(PlayerScript player)
    {
        player.
        miniGame.
        MiniGameDone.
        AddListener(OnMiniGameEnd);
    }

    public void OnMiniGameEnd()
    {
        if (afterMiniGameAudioClip != null)
        {
            SoundManager.Instance.PlaySound(afterMiniGameAudioClip, afterMiniGameAudioClipVolume);
        }
        chosenPlayer.miniGame.MiniGameDone.RemoveListener(OnMiniGameEnd);
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
    public void ChoosePlayer(PlayerScript playerScript)//chooses player and sets not chosen player
    {
        chosenPlayer = playerScript;

        if (chosenPlayer != playerL)
        {
            unchosenPlayerPrefab = playerR.selfPrefab;
            Debug.Log("unchosenPlayer = playerR.selfPrefab;");
        }

        if (chosenPlayer != playerR)
        {
            unchosenPlayerPrefab = playerL.selfPrefab;
            Debug.Log("unchosenPlayer = playerL.selfPrefab;");
        }
    }
    public void PickSet(GameObject set)
    {
        selectedStudioSetInMenu = set.GetComponent<StudioSet>();
        /*         for (int i = 0; i < studioSetList.Length; i++)
                {
                    if (studioSetList[i].gameObject != set)
                    {
                        studioSetList[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        selectedStudioSetInMenu = studioSetList[i];
                        studioSetList[i].gameObject.SetActive(true);
                    }
                } */
    }

    public void ApplySelectedStudioSet()
    {
        for (int i = 0; i < studioSetList.Length; i++)
        {
            if (studioSetList[i] != selectedStudioSetInMenu)
            {
                studioSetList[i].gameObject.SetActive(false);
            }
            else
            {
                studioSet = selectedStudioSetInMenu;
                studioSetList[i].gameObject.SetActive(true);
            }
        }
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

    public void SwitchToRespectivePlayersMiniGameCamera()
    {
        CameraController.Instance.transitionToCMVirtualCamera(GameController.Instance.ChosenPlayer.miniGame.miniGameCam);
    }

    public void SwitchToQuestioningMusic()
    {
        if (studioSet.questioningMusic != null)
        {
            SoundManager.Instance.SwitchBackgroundMusic(studioSet.questioningMusic, studioSet.questioningMusicVolume);
        }
    }

    public void SwitchToFinalMusic()
    {
        if (studioSet.finalMusic != null)
        {
            SoundManager.Instance.SwitchBackgroundMusic(studioSet.finalMusic, studioSet.finalMusicVolume);
        }
    }

    public void SetCoinAmount(int x)
    {
        coinAmount = x;
        CoinAmountUpdated?.Invoke();
    }
}
