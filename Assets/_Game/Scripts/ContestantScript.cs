using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using PixelCrushers;
using PixelCrushers.DialogueSystem;


public class ContestantScript : MonoBehaviour
{
    public CinemachineVirtualCamera cam = null;
    public Sprite thumbsUpSprite = null;
    public Sprite thumbsDownSprite = null;

    public Image thumbsUpOrDownImage = null;

    public GameObject hitboxForSelection = null;
    public Outline selectedIndicator = null;

    public DialogueSystemTrigger dialogueSystemTrigger=null;

    public GameObject hole=null;

    private bool isSelected = false;

    public bool IsSelected { get => isSelected; set => isSelected = value; }

    private GameController gameController;

    private void Awake() {
        selectedIndicator.enabled=false;
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
        selectedIndicator.enabled=isSelected;

        return isSelected;
    }

    public void Eliminate(){
        
    }
}
