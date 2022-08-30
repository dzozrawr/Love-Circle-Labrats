using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DogMiniGame
{

    public enum DogCommand
    {
        Dance, RollOver, Sit, Turn
    }
    public class DogMiniGameManager : MonoBehaviour
    {
        private static float delayAfterMiniGameDone = 1f;

        public GameObject commandsToDoGroup = null;
        public Animator dogAnimator = null;
        //public DogCommandToDo[] commandsToDo=null;
        public GameObject commandUserButtonsGroup = null;


        private Queue<DogCommandToDo> dogCommandQueue = new Queue<DogCommandToDo>();
        private Button[] commandUserButtons;

        private bool dogInAnimation=false;
        private void Awake()
        {
            
        }

        private void Start()
        {
            if (commandsToDoGroup != null)
            {
                //  DogCommandToDo[] dArray= commandsToDoGroup.transform.GetChild();

                for (int i = 0; i < commandsToDoGroup.transform.childCount; i++)
                {
                    dogCommandQueue.Enqueue(commandsToDoGroup.transform.GetChild(i).GetComponent<DogCommandToDo>());
                    // Debug.Log(dogCommandQueue.Peek().dogCommand);
                }
            }

            commandUserButtons = commandUserButtonsGroup.GetComponentsInChildren<Button>();
           // Debug.Log(commandUserButtons.Length);
        }


        private void Update() {
/*             if(dogInAnimation){
                 if (dogAnimator.GetCurrentAnimatorStateInfo(0).){
                    SetButtonsInteractable(true);
                    dogInAnimation=false;
                 }
            } */
        }
        public void DogCommandButtonEffect(DogCommandButton dogCommandButton)
        {
            // Debug.Log(dogCommandButton.dogCommand);
            //  Debug.Log(dogCommandQueue.Peek().dogCommand);
            if (dogAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                if (dogCommandButton.dogCommand == dogCommandQueue.Peek().dogCommand)
                {
                    dogCommandQueue.Dequeue().SetAsDone(); //pop the thing //change the sprite
                    DoDogCommand(dogCommandButton.dogCommand);
                    dogCommandButton.GetComponent<Image>().sprite=dogCommandButton.GetComponent<Button>().spriteState.selectedSprite;
                    SetButtonsInteractable(false);
                    StartCoroutine(WaitForIdle());
                    //dogInAnimation=true;
                    // Debug.Log("Correct command!"); //replace with indicator in the game that it is right
                    //play the animation of the dog and owner?
                    dogCommandButton.GetComponent<Button>().transition = Selectable.Transition.None;

                    if (dogCommandQueue.Count == 0)
                    {
                       // Debug.Log("Mini game done!");
                       // Invoke(nameof(HideMiniGame), delayAfterMiniGameDone);
                       StartCoroutine(WaitForIdle(true));
                    }//check if its the last one for the congratulations message
                }
                else
                {
                    // GameObject myEventSystem = GameObject.Find("EventSystem");
                    //  myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
                    //  Debug.Log("Wrong command!");    //replace with indicator in the game that it is wrong
                }
            }

             if (dogAnimator.GetCurrentAnimatorStateInfo(0).IsName("Sit")){
                
             }
        }

        IEnumerator WaitForIdle(bool isMiniGameOver=false){
            yield return new WaitUntil(()=>!dogAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
            //System.Func<bool> a=dogAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle");
            yield return new WaitUntil(()=>dogAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
            if(!isMiniGameOver)
            SetButtonsInteractable(true);
            else HideMiniGame();
        }

        private void SetButtonsInteractable(bool b)
        {
            for (int i = 0; i < commandUserButtons.Length; i++)
            {
                commandUserButtons[i].enabled=b;
            }
        }

        private void DoDogCommand(DogCommand command)
        {
            switch (command)
            {
                case DogCommand.Sit:
                    dogAnimator.SetTrigger("Sit");
                    break;
                case DogCommand.Dance:
                    dogAnimator.SetTrigger("Dance");
                    break;
                case DogCommand.RollOver:
                    dogAnimator.SetTrigger("RollOver");
                    break;
                case DogCommand.Turn:
                    dogAnimator.SetTrigger("Spin");
                    break;
            }

        }
        private void HideMiniGame()
        {
            Destroy(gameObject);
        }
    }



}
