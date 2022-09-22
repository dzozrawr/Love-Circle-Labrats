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

    protected override void Start()
    {
        base.Start();

        int musicInt = PlayerPrefs.GetInt("music", -1);
        if (musicInt == -1)
        {
            RefreshButtonStatus(true);
        }
        else
        {
            RefreshButtonStatus(musicInt == 1);
        }

        SoundManager.Instance.MusicToggle=this;
        valueChanged += SoundManager.Instance.MusicToggle_valueChanged;
    }
    protected override void SpecificToggleEffect()
    {
        PlayerPrefs.SetInt("music", _isOn ? 1 : 0);
    }
}
