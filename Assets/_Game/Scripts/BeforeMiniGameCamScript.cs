using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

[RequireComponent(typeof(DialogueSystemTrigger))]
public class BeforeMiniGameCamScript : MonoBehaviour
{
    private GameController gameController;
    private DialogueSystemTrigger dialogueSystemTrigger;

    // Start is called before the first frame update
    void Start()
    {
        gameController=GameController.Instance;
        gameController.OnConversationChanged+=OnConversationChanged;

        dialogueSystemTrigger=GetComponent<DialogueSystemTrigger>();
    }
    private void OnConversationChanged(string conversationName){
        dialogueSystemTrigger.conversation="BeforeMiniGame "+conversationName;
    }
}
