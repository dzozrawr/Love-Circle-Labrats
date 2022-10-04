using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;
using PathCreation;
using PathCreation.Examples;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Tabtale.TTPlugins;

public class GameController : MonoBehaviour //all of the events are in this class
{
    private static GameController instance = null;
    public static GameController Instance { get => instance; }

    public static int missionID;

    private static GameObject unchosenPlayerPrefab = null;

    public delegate void ConversationChangeHandler(string conversationName);
    public event ConversationChangeHandler OnConversationChanged;

    private PlayerScript chosenPlayer = null;
    public PlayerScript ChosenPlayer { get => chosenPlayer; set => chosenPlayer = value; }
    public static int CoinAmount { get => coinAmount; set => coinAmount = value; }
    public static GameObject UnchosenPlayerPrefab { get => unchosenPlayerPrefab; set => unchosenPlayerPrefab = value; }

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
    private static int coinAmount=0;




    //public PlayerScript leftPlayer = null, rightPlayer=null;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        //reading persistent data
       // unchosenPlayerPrefab = PersistentData.unchosenPlayerPrefab;

        selectedStudioSetInMenu = studioSet;
        //coinAmount = 0;

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
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            unchosenPlayerPrefab = null;
          //  PersistentData.unchosenPlayerPrefab = null;
        }
        if (unchosenPlayerPrefab != null)
        {
            AddUnchosenPlayer();
        }

        Dictionary<string, object> parametersForClik = new Dictionary<string, object>();

        parametersForClik.Add("missionName", SceneManager.GetActiveScene().name);
        parametersForClik.Add("CoinBalance", coinAmount);
        TTPGameProgression.FirebaseEvents.MissionStarted(missionID, parametersForClik); //CHANGE THIS NUMBER IF YOU DELETE QUICKCLIK
       // lastMissionStartedID = missionID;
    }



    public void AddUnchosenPlayer()
    {
        //  Debug.Log("AddUnchosenPlayer()");
        GameObject go = Instantiate(unchosenPlayerPrefab, startingPlayerRTransform.position, Quaternion.identity);
        playerR = go.GetComponent<PlayerScript>();

        playerR.miniGame = Instantiate(playerR.miniGamePrefab);

        //= go.GetComponent<MiniGame>();
        playerR.GetComponent<PathFollower>().pathCreator = playerPathR;

        PlayerPickingButton playerPickingButtonR = GameCanvasController.Instance.playerPickingButtonR;
        playerPickingButtonR.player = playerR;
        playerPickingButtonR.GetComponent<Image>().sprite = playerR.buttonIcon;
        playerPickingButtonR.nameImage.sprite = playerR.playerNameSprite;
        playerPickingButtonR.descriptonText.text = playerR.playerDescriptonString;
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

#if UNITY_EDITOR
    // Update is called once per frame
    void Update()
    {



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
            //  CameraController.Instance.transitionToCMVirtualCamera(CameraController.CameraPhase.DogMiniGame);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            // CameraController.Instance.transitionToCMVirtualCamera(CameraController.CameraPhase.BakingMiniGame);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            CameraController.Instance.transitionToCMVirtualCamera(CameraController.CameraPhase.ContestantsElimination);
        }

        
        if (Input.GetKeyDown(KeyCode.M))
        {
            SetCoinAmount(CoinAmount+200);
        }


    }
    #endif
    public void ChoosePlayer(PlayerScript playerScript)//chooses player and sets not chosen player
    {
        chosenPlayer = playerScript;

        if (playerR == null)
        {
            Debug.Log("PlayerR is not set!");
            return;
        }

        if (chosenPlayer == playerL)
        {
            unchosenPlayerPrefab = playerR.selfReferencePrefabHolder.prefabRefence;
           // PersistentData.unchosenPlayerPrefab = unchosenPlayerPrefab;
            //            Debug.Log("unchosenPlayer = playerR.selfPrefab;");
        }

        if (chosenPlayer == playerR)
        {
            unchosenPlayerPrefab = playerL.selfReferencePrefabHolder.prefabRefence;
           // PersistentData.unchosenPlayerPrefab = unchosenPlayerPrefab;
            //   Debug.Log("unchosenPlayer = playerL.selfPrefab;");
        }
    }

#if UNITY_EDITOR
    public void SetUnchosenPlayer(PlayerScript playerScript)
    {
        unchosenPlayerPrefab = playerScript.selfReferencePrefabHolder.prefabRefence;
       // PersistentData.unchosenPlayerPrefab = unchosenPlayerPrefab;
    }
#endif
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

    public static bool IsOverRaycastBlockingUI()
    {
        int id = 0;
#if UNITY_EDITOR
        id = -1;
#endif
        //  bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(id);       //this checks if the pointer is over UI (through EventSystem) and if it is then it blocks raycasts
        bool isOverBlockingUI =
                                UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(id) &&
                                UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject != null &&
                                UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.CompareTag("UIRayBlock");       //this checks if the pointer is over UI (through EventSystem) and if it is then it blocks raycasts

        return isOverBlockingUI;
    }
}
