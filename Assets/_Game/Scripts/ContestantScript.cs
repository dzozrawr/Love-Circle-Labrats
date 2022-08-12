using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
using DG.Tweening;


public class ContestantScript : MonoBehaviour
{
    public CinemachineVirtualCamera cam = null;
    public Sprite thumbsUpSprite = null;
    public Sprite thumbsDownSprite = null;

    public Image thumbsUpOrDownImage = null;

    public GameObject hitboxForSelection = null;
    public Outline selectedIndicator = null;

    public DialogueSystemTrigger dialogueSystemTrigger = null;

    public GameObject hole = null;

    public Animator animator = null;

    private Vector3 holePreferredScale;

    private bool isSelected = false;

    public bool IsSelected { get => isSelected; set => isSelected = value; }

    private GameController gameController;

    private void Awake()
    {
        selectedIndicator.enabled = false;
        /*         if (hole != null)
                {
                    holePreferredScale = hole.transform.localScale;
                    hole.transform.localScale = new Vector3(0, hole.transform.localScale.y, 0);
                } */
    }

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameController.Instance;
        gameController.OnConversationChanged += OnConversationChanged;


    }

    private void OnConversationChanged(string conversationName)
    {
        dialogueSystemTrigger.conversation = conversationName;
        // Debug.LogError(gameObject.name + " conversation changed to " + conversationName);
    }

    public void SetThumbsUpOrDown(bool isThumbsUp)
    {
        if (isThumbsUp)
        {
            thumbsUpOrDownImage.sprite = thumbsUpSprite;
        }
        else
        {
            thumbsUpOrDownImage.sprite = thumbsDownSprite;
        }
        thumbsUpOrDownImage.gameObject.SetActive(true);
    }

    public void ToggleSelectionPhase(bool onOff)
    {
        hitboxForSelection.SetActive(onOff);

    }

    public bool Select()
    {
        isSelected = !isSelected;
        selectedIndicator.enabled = isSelected;

        return isSelected;
    }

    public void Eliminate()
    {
        selectedIndicator.enabled = false;
        animator.SetTrigger("Amaze");

        //hole.transform.DOLocalMoveY(hole.GetComponent<Renderer>().bounds.size.y,0.5f);
        // hole.transform.DOMove(hole.transform.position+hole.transform.up*hole.GetComponent<Renderer>().bounds.size.y,1f);
        hole.transform.DOMove(hole.transform.position + hole.transform.up * hole.GetComponent<Renderer>().bounds.size.x, 0.5f).onComplete = () =>
        {
            Invoke(nameof(DropTweenAnimationAfterDelay), 0.5f);
        };

        /*         hole.transform.DOScale(holePreferredScale, 0.5f).onComplete = () =>
                {
                    Invoke(nameof(DropTweenAnimationAfterDelay), 0.5f);
                }; */
        // play animation of getting scared
        // add/activate trail
        // animation of falling down

    }

    private void DropTweenAnimationAfterDelay()
    {
        gameObject.transform.DOMoveY(-5f, 0.25f).onComplete = () =>
        {
            ContestantQuestioningManager.Instance.ContestantEliminatedSignal();
/*             hole.transform.DOScale(new Vector3(0, hole.transform.localScale.y, 0), 0.5f).onComplete = () =>
            {
                
            }; */
        };
    }
}
