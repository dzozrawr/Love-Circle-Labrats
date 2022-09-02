using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameCanvasController : MonoBehaviour
{
    private static GameCanvasController instance = null;

    public static GameCanvasController Instance { get => instance; }

    public GameObject thumbsUpDownButtonGroup = null;
    public Button eliminateButton = null;
    public GameObject choosePlayerButtonGroup = null;
    public GameObject mainMenuGroup = null;
    public GameObject setPickingGroup=null;
    public GameObject EOLScreen=null;
    public Button endEpisodeButton=null;

    public GameObject successfulMatchGroup = null, goodMatchGroup = null, terribleMatchGroup = null;

    private GameController gameController = null;
    private CameraController cameraController = null;
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

        //defining default UI state at the beginning of the game below
        setPickingGroup.SetActive(false);
    }

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
        button.player.ChoosePlayer();
        choosePlayerButtonGroup.GetComponent<Animator>().SetTrigger("Hide");
    }

    public void PlayButtonEffect()
    {
        mainMenuGroup.GetComponent<Animator>().SetTrigger("Hide");
        cameraController.transitionToCMVirtualCamera(CameraController.CameraPhase.Intro);
    }

    public void SetPickingButtonEffect(){
        setPickingGroup.SetActive(true);
        setPickingGroup.GetComponent<Animator>().SetTrigger("Show");
    }

    public void NextLevelButtonEffect(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ActivateEOLScreenBasedOnMatchSuccessRate(float successRate)
    {
        EOLScreen.SetActive(true);

        if (successRate == 0f)  //kind of hard coded
        {
            terribleMatchGroup.SetActive(true);
        }else if (successRate == 0.5f)
        {
            goodMatchGroup.SetActive(true);
        }else if (successRate == 1f)
        {
            successfulMatchGroup.SetActive(true);
        }
    }
}
