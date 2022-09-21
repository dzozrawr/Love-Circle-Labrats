using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using NiceVibrations.CrazyLabsExtension;

public class AudioToggle : SettingsToggle
{
    private static AudioToggle instance;
    public static AudioToggle Instance { get => instance; }


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);    //this line of code shouldnt happen ever
            return;
        }
        instance = this;
    }



    private void OnEnable() {

    }


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        int audioInt = PlayerPrefs.GetInt("audio", -1);
        if (audioInt == -1)
        {
            RefreshButtonStatus(true);
        }
        else
        {
            RefreshButtonStatus(audioInt == 1);
        }

        SoundManager.Instance.SwitchForAudio=this;
        valueChanged += SoundManager.Instance.SwitchForAudio_valueChanged;
        //  _isOn = SoundManager.Instance.IsAudioSourceEnabled;
        // RefreshButtonStatus(SoundManager.Instance.IsAudioSourceEnabled);
        ///Toggle(!isOn, false);
    }
    /*    RefreshButtonStatus(HapticFeedbackController.IsHapticsEnabled);

    }
    */
    protected override void SpecificToggleEffect()
    {
        PlayerPrefs.SetInt("audio", _isOn ? 1 : 0);
    }


}
