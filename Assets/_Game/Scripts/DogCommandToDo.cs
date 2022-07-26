using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DogMiniGame;
using UnityEngine.UI;

[RequireComponent (typeof(Image))]
public class DogCommandToDo : MonoBehaviour
{
    public DogCommand dogCommand;

    public Sprite successSprite = null;

    private Sprite imageSprite = null;
    private void Awake()
    {
        imageSprite = GetComponent<Image>().sprite;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetAsDone()
    {
        imageSprite = successSprite;
        //play particles or fireworks or whatever
    }

}
