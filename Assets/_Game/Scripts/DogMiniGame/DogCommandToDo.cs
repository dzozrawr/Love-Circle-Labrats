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
