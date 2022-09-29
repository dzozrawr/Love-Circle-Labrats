using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoicesToggle : SettingsToggle
{

    protected override void Start()
    {
        base.Start();

        int voicesInt = PlayerPrefs.GetInt("voices", -1);
        if (voicesInt == -1)
        {
            RefreshButtonStatus(true);
        }
        else
        {
            RefreshButtonStatus(voicesInt == 1);
        }

        SoundManager.Instance.VoicesToggle = this;
        valueChanged += SoundManager.Instance.VoicesToggle_valueChanged;
    }

    protected override void SpecificToggleEffect()
    {
        PlayerPrefs.SetInt("voices", _isOn ? 1 : 0);
    }
}
