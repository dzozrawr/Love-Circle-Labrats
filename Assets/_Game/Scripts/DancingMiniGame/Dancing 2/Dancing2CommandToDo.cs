using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dancing2MiniGame;
using UnityEngine.UI;

[RequireComponent (typeof(Image))]
public class Dancing2CommandToDo : MonoBehaviour
{
    public Dancing2Command dancingCommand;

    public Sprite successSprite = null;

    private Image image = null;
    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    public void SetAsDone()
    {
        image.sprite = successSprite;
        //play particles or fireworks or whatever
    }

}
