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
        //public DogCommandToDo[] commandsToDo=null;

        private Queue<DogCommandToDo> dogCommandQueue=new Queue<DogCommandToDo>();
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
        }
        public void DogCommandButtonEffect(DogCommandButton dogCommandButton)
        {
           // Debug.Log(dogCommandButton.dogCommand);
          //  Debug.Log(dogCommandQueue.Peek().dogCommand);
            if (dogCommandButton.dogCommand == dogCommandQueue.Peek().dogCommand)
            {
                dogCommandQueue.Dequeue().SetAsDone(); //pop the thing //change the sprite
               // Debug.Log("Correct command!"); //replace with indicator in the game that it is right
                //play the animation of the dog and owner?
                dogCommandButton.GetComponent<Button>().transition=Selectable.Transition.None;

                if (dogCommandQueue.Count == 0)
                {
                    Debug.Log("Mini game done!");
                    Invoke(nameof(HideMiniGame),delayAfterMiniGameDone);
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
        private void HideMiniGame()
        {
            Destroy(gameObject);
        }
    }



}
