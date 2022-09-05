using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicToggle : SettingsToggle
{
    private static MusicToggle instance;
    public static MusicToggle Instance { get => instance; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);    //this line of code shouldnt happen ever
            return;
        }
        instance = this;
    }

    private void Start()
    {
        toggleIndicatorImage = toggleIndicator.gameObject.GetComponent<Image>();

        offX = toggleIndicator.anchoredPosition.x;
        onX = backgroundImage.rectTransform.rect.width - toggleIndicator.rect.width;

        audioSource = this.GetComponent<AudioSource>();

        int musicInt = PlayerPrefs.GetInt("music", -1);
        if (musicInt == -1)
        {
            RefreshButtonStatus(true);
        }
        else
        {
            RefreshButtonStatus(musicInt == 1);
        }
    }
    protected override void SpecificToggleEffect()
    {
        PlayerPrefs.SetInt("music", _isOn ? 1 : 0);
    }
}
