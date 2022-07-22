using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvasController : MonoBehaviour
{
    private static GameCanvasController instance = null;

    public static GameCanvasController Instance { get => instance; }


    public GameObject thumbsUpDownButtonGroup = null;
    public Button eliminateButton = null;

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
    }

    public void ThumbsUpDownButtonEffect(bool isThumbsUp)
    {
        ContestantQuestioningManager.Instance.CurContestant.SetThumbsUpOrDown(isThumbsUp);
        ShowThumbsUpDown(false);
        Invoke(nameof(MoveToNextContestant), 0.5f);
    }

    private void MoveToNextContestant()
    {
        ContestantQuestioningManager.Instance.MoveToNextContestant();
    }
}
