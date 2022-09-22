using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameCanvasController : MonoBehaviour
{
    private static GameCanvasController instance = null;

    public static GameCanvasController Instance { get => instance; }

    

    public GameObject thumbsUpDownButtonGroup = null;
    public Button eliminateButton = null;
    public GameObject choosePlayerButtonGroup = null;

    public PlayerPickingButton playerPickingButtonR = null;

    public GameObject mainMenuGroup = null;
    public GameObject setPickingGroup = null;
    public GameObject settingsGroup = null;
    public GameObject EOLScreen = null;
    public Button endEpisodeButton = null;
    public GameObject coinUI = null;

    public AudioClip onPlayerPickAudioClip = null;
    [Range(0f, 1f)]
    public float onPlayerPickAudioClipVolume = 1f;

    //public GameObject successfulMatchGroup = null, goodMatchGroup = null, terribleMatchGroup = null;
    public AudioClip successfulMatchAudioClip = null;
    [Range(0f, 1f)]
    public float successfulMatchAudioClipVolume = 1f;
    public AudioClip goodMatchAudioClip = null;
    [Range(0f, 1f)]
    public float goodMatchAudioClipVolume = 1f;
    public AudioClip terribleMatchAudioClip = null;
    [Range(0f, 1f)]
    public float terribleMatchAudioClipVolume = 1f;

    private GameController gameController = null;
    private CameraController cameraController = null;

    private TMP_Text coinAmountTxt = null;
    private ContestantQuestioningManager contestantQuestioningManager = null;

    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        
    }

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameController.Instance;
        cameraController = CameraController.Instance;
        contestantQuestioningManager = ContestantQuestioningManager.Instance;

        coinAmountTxt = coinUI.GetComponentInChildren<TMP_Text>();
        coinAmountTxt.text = GameController.CoinAmount + "";
        gameController.CoinAmountUpdated.AddListener(UpdateCoinAmountUI);

        //defining default UI state at the beginning of the game below
        setPickingGroup.SetActive(false);
        settingsGroup.SetActive(false);
        EOLScreen.SetActive(false);

        coinUI.GetComponent<Animation>().Play("Coin UI Show");
    }
#if UNITY_EDITOR
/*     private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EOLScreen.SetActive(true);
            terribleMatchGroup.SetActive(true);
            coinUI.GetComponent<Animation>().Play("Coin UI Show");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EOLScreen.SetActive(true);
            goodMatchGroup.SetActive(true);
            coinUI.GetComponent<Animation>().Play("Coin UI Show");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EOLScreen.SetActive(true);
            successfulMatchGroup.SetActive(true);
            coinUI.GetComponent<Animation>().Play("Coin UI Show");
        }

        if(Input.GetKeyDown(KeyCode.Space)){
            EndEpisodeButtonEffect();
        }

    } */
#endif

    public void ShowThumbsUpDown(bool show)
    {
        if (show)
        {
            thumbsUpDownButtonGroup.SetActive(true);
        }
        else
        {
            thumbsUpDownButtonGroup.SetActive(false);
        }
    }

    public void ToggleEliminateButtonVisibility(bool show)
    {
        eliminateButton.gameObject.SetActive(show);
    }



    public void ThumbsUpDownButtonEffect(bool isThumbsUp)
    {
        contestantQuestioningManager.CurContestant.SetThumbsUpOrDown(isThumbsUp);
        ShowThumbsUpDown(false);
        Invoke(nameof(MoveToNextContestant), 0.5f);
    }

    public void AnotherQuestionButtonEffect(){
        ShowThumbsUpDown(false);
        contestantQuestioningManager.CurContestant.RestartConversation();
    }

    public void ShowPlayerPickingButtons()
    {

        choosePlayerButtonGroup.SetActive(true);

    }

    private void MoveToNextContestant()
    {
        contestantQuestioningManager.MoveToNextContestant();
    }

    public void ChoosePlayerButtonEffect(PlayerPickingButton button)
    {
        foreach (PlayerPickingButton b in choosePlayerButtonGroup.GetComponentsInChildren<PlayerPickingButton>())
        {
            b.GetComponent<Button>().enabled = false;
        }

        button.player.ChoosePlayer();
        choosePlayerButtonGroup.GetComponent<Animator>().SetTrigger("Hide");

        if (onPlayerPickAudioClip != null)
        {
            SoundManager.Instance.PlaySound(onPlayerPickAudioClip, onPlayerPickAudioClipVolume);
        }
    }

    public void PlayButtonEffect()
    {
        mainMenuGroup.GetComponent<Animator>().SetTrigger("Hide");
        coinUI.GetComponent<Animation>().Play("Coin UI Hide");
        cameraController.transitionToCMVirtualCamera(CameraController.CameraPhase.Intro);
    }

    public void SetPickingButtonEffect()
    {
        setPickingGroup.SetActive(true);
        setPickingGroup.GetComponent<Animator>().SetTrigger("Show");
    }

    public void SettingsButtonEffect()
    {
        settingsGroup.SetActive(true);
        settingsGroup.GetComponent<Animator>().SetTrigger("Show");
    }

    public void EndEpisodeButtonEffect()
    {
//        Debug.Log("EndEpisodeButtonEffect()");
        int nextSceneIndex=(SceneManager.GetActiveScene().buildIndex+1)%SceneManager.sceneCountInBuildSettings;
        if(nextSceneIndex==0) nextSceneIndex++;

        SaveData saveData=new SaveData(nextSceneIndex);
        SaveSystem.SaveGame(saveData);

        SceneManager.LoadScene(nextSceneIndex);
    }

    public void ActivateEOLScreenBasedOnMatchSuccessRate(float successRate)
    {
        EOLScreen.SetActive(true);
        coinUI.GetComponent<Animation>().Play("Coin UI Show");

        EOLScreen.GetComponent<EOLScreenController>().ActivateEOLScreenBasedOnMatchSuccessRate(successRate);
 /*        if (successRate == 0f)  //kind of hard coded
        {
            terribleMatchGroup.SetActive(true);
            if (terribleMatchAudioClip != null)
            {
                SoundManager.Instance.PlaySound(terribleMatchAudioClip,terribleMatchAudioClipVolume);
            }
        }
        else if (successRate == 0.5f)
        {
            goodMatchGroup.SetActive(true);
            if (goodMatchAudioClip != null)
            {
                SoundManager.Instance.PlaySound(goodMatchAudioClip,goodMatchAudioClipVolume);
            }
        }
        else if (successRate == 1f)
        {
            successfulMatchGroup.SetActive(true);
            if (successfulMatchAudioClip != null)
            {
                SoundManager.Instance.PlaySound(successfulMatchAudioClip,successfulMatchAudioClipVolume);
            }
        } */
    }
    public void SetActiveFalse()
    {
        setPickingGroup.SetActive(false);
        settingsGroup.SetActive(false);
    }

    public void InvokeSetActiveFalse()
    {
        Invoke(nameof(SetActiveFalse), 0.25f);
    }

    public void UpdateCoinAmountUI()
    {
        coinAmountTxt.text = GameController.CoinAmount + "";
    }

}
