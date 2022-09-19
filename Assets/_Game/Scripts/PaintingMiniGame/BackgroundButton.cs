using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(Image))]
public class BackgroundButton : MonoBehaviour
{
    private Button button = null;
    private Image image = null;

    public Sprite unselectedSprite = null;
    public Sprite selectedSprite = null;
    public Texture paintingTexture = null;

    public Button Button { get => button; set => button = value; }
    public Image Image { get => image; set => image = value; }

    private void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
    }

    public void OnClickButtonEffect()
    {
        GetComponent<Image>().sprite = selectedSprite;

        foreach (BackgroundButton b in transform.parent.GetComponentsInChildren<BackgroundButton>())
        {
            if (b == this) continue;
            b.SetSelectedAppearance(false);
        }
        //button.enabled = false;

        PaintingMiniGame.Instance.SetBackgroundTexture(paintingTexture);
    }

    public void SetSelectedAppearance(bool shouldEnable)
    {
        if (shouldEnable)
        {
            Image.sprite = selectedSprite;
        }
        else
        {
            Image.sprite = unselectedSprite;
        }
    }
}
