using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DogMiniGame
{

    public enum DogCommand
    {
        Dance, RollOver, Sit, Turn
    }
    public class DogMiniGameManager : MonoBehaviour
    {
        // public GameObject commandsToDoGroup = null;
        public DogCommandToDo[] commandsToDo=null;

        private Stack<DogCommandToDo> dogCommandStack=new Stack<DogCommandToDo>();
        private void Awake()
        {
            
        }

        private void Start()
        {
            /*            if (commandsToDoGroup != null)
                        {
                          //  DogCommandToDo[] dArray= commandsToDoGroup.transform.GetChild();

                            for (int i = 0; i < commandsToDoGroup.transform.childCount; i++)
                            {
                                dogCommandStack.Push(commandsToDoGroup.transform.GetChild(i).GetComponent<DogCommandToDo>());
                                Debug.Log(dogCommandStack.Peek().dogCommand);
                            }
                        }*/

            if (commandsToDo != null)
            {
                //  DogCommandToDo[] dArray= commandsToDoGroup.transform.GetChild();

                for (int i = 0; i < commandsToDo.Length; i++)
                {
                    Debug.Log(commandsToDo[i].dogCommand);
                    dogCommandStack.Push(commandsToDo[i]);
                    //Debug.Log(dogCommandStack.Peek().dogCommand);
                   
                }
            }
        }
        public void DogCommandButtonEffect(DogCommandButton dogCommandButton)
        {
            Debug.Log(dogCommandButton.dogCommand);
            Debug.Log(dogCommandStack.Peek().dogCommand);
            if (dogCommandButton.dogCommand == dogCommandStack.Peek().dogCommand)
            {
                dogCommandStack.Pop().SetAsDone(); //pop the thing
                //change the sprite

                //check if its the last one for the congratulations message
            }
            else
            {
                Debug.Log("Wrong command!");
            }
        }
    }



}
