using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class ContestantScript : MonoBehaviour
{
    public CinemachineVirtualCamera cam = null;
    public Sprite thumbsUpSprite = null;
    public Sprite thumbsDownSprite = null;

    public Image thumbsUpOrDownImage = null;
    // Start is called before the first frame update
    void Start()
    {
        
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
}
