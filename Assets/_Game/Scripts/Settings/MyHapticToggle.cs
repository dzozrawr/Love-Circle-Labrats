using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using NiceVibrations.CrazyLabsExtension;

public class MyHapticToggle : SettingsToggle
{


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();


        RefreshButtonStatus(HapticFeedbackController.IsHapticsEnabled);

    }

    protected override void SpecificToggleEffect()
    {
        HapticFeedbackController.ToggleHaptics();
    }

    /*    private void ToggleColor(bool value)
        {
            if (value) toggleIndicatorImage.DOColor(onColor, tweenTime);
            else
                toggleIndicatorImage.DOColor(offColor, tweenTime);
        }*/
}
