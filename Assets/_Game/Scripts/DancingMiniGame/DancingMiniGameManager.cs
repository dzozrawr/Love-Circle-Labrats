using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DancingMiniGame
{

    public enum DancingCommand
    {
        Salsa, Twerk, Hiphop, Maraschino
    }
    public class DancingMiniGameManager : MonoBehaviour
    {
        private static float delayAfterMiniGameDone = 1f;

        public GameObject commandsToDoGroup = null;
        public Animator dancingAnimator = null;
        //public DogCommandToDo[] commandsToDo=null;
        public GameObject commandUserButtonsGroup = null;

        public DancingMiniGameM dancingMiniGameM = null;


        private Queue<DancingCommandToDo> dancingCommandQueue = new Queue<DancingCommandToDo>();
        private Button[] commandUserButtons;

        private bool dancingInAnimation = false;
        private GameController gameController=null;


        private void Start()
        {
            if (commandsToDoGroup != null)
            {
                //  DogCommandToDo[] dArray= commandsToDoGroup.transform.GetChild();

                for (int i = 0; i < commandsToDoGroup.transform.childCount; i++)
                {
                    dancingCommandQueue.Enqueue(commandsToDoGroup.transform.GetChild(i).GetComponent<DancingCommandToDo>());
                    // Debug.Log(dogCommandQueue.Peek().dogCommand);
                }
            }

            commandUserButtons = commandUserButtonsGroup.GetComponentsInChildren<Button>();
            gameController=GameController.Instance;
            // Debug.Log(commandUserButtons.Length);
        }



        public void DancingCommandButtonEffect(DancingCommandButton dancingCommandButton)
        {
            // Debug.Log(dogCommandButton.dogCommand);
            //  Debug.Log(dogCommandQueue.Peek().dogCommand);
            if (dancingAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                if (dancingCommandButton.dancingCommand == dancingCommandQueue.Peek().dancingCommand)
                {
                    dancingCommandQueue.Dequeue().SetAsDone(); //pop the thing //change the sprite
                    DoDancingCommand(dancingCommandButton.dancingCommand);
                    dancingCommandButton.GetComponent<Image>().sprite = dancingCommandButton.GetComponent<Button>().spriteState.selectedSprite;
                    SetButtonsInteractable(false);
                    StartCoroutine(WaitForIdle());
                    //dogInAnimation=true;
                    // Debug.Log("Correct command!"); //replace with indicator in the game that it is right
                    //play the animation of the dog and owner?
                    dancingCommandButton.GetComponent<Button>().transition = Selectable.Transition.None;

                    if (dancingCommandQueue.Count == 0)
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
        }

        IEnumerator WaitForIdle(bool isMiniGameOver = false)
        {
            yield return new WaitUntil(() => !dancingAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
            //System.Func<bool> a=dogAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle");
            yield return new WaitUntil(() => dancingAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
            if (!isMiniGameOver)
                SetButtonsInteractable(true);
            else
            {
                gameObject.GetComponent<Animation>().Play();
                dancingMiniGameM.TransitionToContestants();
                if (gameController.afterMiniGameAudioClip != null)
                {
                    SoundManager.Instance.PlaySound(gameController.afterMiniGameAudioClip, gameController.afterMiniGameAudioClipVolume);
                }
                Invoke(nameof(HideMiniGame), 2f);
            }
        }

        private void SetButtonsInteractable(bool b)
        {
            for (int i = 0; i < commandUserButtons.Length; i++)
            {
                commandUserButtons[i].enabled = b;
            }
        }

        private void DoDancingCommand(DancingCommand command)
        {
            switch (command)
            {
                case DancingCommand.Salsa:
                    dancingAnimator.SetTrigger("Salsa");
                    break;
                case DancingCommand.Twerk:
                    dancingAnimator.SetTrigger("Twerk");
                    break;
                case DancingCommand.Hiphop:
                    dancingAnimator.SetTrigger("Hiphop");
                    break;
                case DancingCommand.Maraschino:
                    dancingAnimator.SetTrigger("Maraschino");
                    break;
            }

        }
        private void HideMiniGame()
        {
            Destroy(gameObject);
        }
    }



}
