using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvasController : MonoBehaviour
{
    private static GameCanvasController instance = null;

    public static GameCanvasController Instance { get => instance; }

    private GameController gameController = null;

    public GameObject thumbsUpDownButtonGroup = null;
    public Button eliminateButton = null;
    public GameObject choosePlayerButtonGroup = null;

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

    public void EliminateButtonEffect()
    {
        ContestantQuestioningManager.Instance.EliminateSelectedContestants();
        CameraController.Instance.transitionToCMVirtualCamera(CameraController.CameraPhase.DogMiniGame);
    }

    public void ThumbsUpDownButtonEffect(bool isThumbsUp)
    {
        ContestantQuestioningManager.Instance.CurContestant.SetThumbsUpOrDown(isThumbsUp);
        ShowThumbsUpDown(false);
        Invoke(nameof(MoveToNextContestant), 0.5f);
    }

    public void ShowPlayerPickingButtons(bool shouldShow)
    {

        choosePlayerButtonGroup.SetActive(shouldShow);

    }

    private void MoveToNextContestant()
    {
        ContestantQuestioningManager.Instance.MoveToNextContestant();
    }

    public void ChoosePlayerButtonEffect(PlayerPickingButton button)
    {
        button.player.ChoosePlayer();
        ShowPlayerPickingButtons(false);
    }
}
