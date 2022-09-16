using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StencilButton : MonoBehaviour
{
    public Sprite unselectedSprite = null;
    public Sprite selectedSprite = null;

    public Texture invertedTexture = null;
    public GameObject stencilGameObject = null;
    public Color brushColor;

    private Button button = null;
    private Image image = null;

    public Image Image { get => image; set => image = value; }
    public Button Button { get => button; set => button = value; }

    private void Awake()
    {
        button = GetComponent<Button>();
        Image = GetComponent<Image>();
    }

    public void OnClickButtonEffect()
    {
        GetComponent<Image>().sprite = selectedSprite;

        foreach (StencilButton b in transform.parent.GetComponentsInChildren<StencilButton>())
        {
            if (b == this) continue;
            b.SetSelectedAppearance(false);
           // b.Image.color = new Color(b.Image.color.r, b.Image.color.g, b.Image.color.b, 0.5f);
           // b.Button.enabled = false;
        }
        //button.enabled = false;

        PaintingMiniGame.Instance.SetStencil(invertedTexture,stencilGameObject,brushColor);
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
