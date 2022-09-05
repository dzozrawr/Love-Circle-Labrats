using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using NiceVibrations.CrazyLabsExtension;

public abstract class SettingsToggle : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] protected bool _isOn = false;

    public bool isOn
    {
        get
        {
            return _isOn;
        }
    }



    [SerializeField]
    protected RectTransform toggleIndicator;

    protected Image toggleIndicatorImage;

    [SerializeField]
    protected Image backgroundImage;

    [SerializeField]
    protected Color onColor;

    [SerializeField]
    protected Color offColor;

    protected float offX;
    protected float onX;

    [SerializeField]
    protected float tweenTime = 0.25f;

    protected AudioSource audioSource;

    public delegate void ValueChanged(bool value);
    public event ValueChanged valueChanged;

    public Text onTxt, offTxt;


    public virtual void OnPointerDown(PointerEventData eventData)
    {
        HapticFeedbackController.TriggerHaptics(MoreMountains.NiceVibrations.HapticTypes.Selection);  //play haptic
        Toggle(!isOn);//flips the switch when clicked
    }

    public void Toggle(bool value, bool playSFX = true)
    {
        if (value != isOn)
        {
            _isOn = value;

            SpecificToggleEffect();
            ToggleColorAndText(isOn);
            MoveIndicator(isOn);

            if (playSFX) audioSource.Play();

            if (valueChanged != null) valueChanged(isOn);
        }
    }

    protected abstract void SpecificToggleEffect();

    private void ToggleColorAndText(bool value)
    {

        if (value)
        {
            backgroundImage.DOColor(onColor, tweenTime);
            onTxt.DOFade(1f, tweenTime);
            offTxt.DOFade(0f, tweenTime);
        }
        else
        {
            backgroundImage.DOColor(offColor, tweenTime);
            onTxt.DOFade(0f, tweenTime);
            offTxt.DOFade(1f, tweenTime);
        }
    }

    private void MoveIndicator(bool value)
    {
        if (value)
            toggleIndicator.DOAnchorPosX(onX, tweenTime);
        else
            toggleIndicator.DOAnchorPosX(offX, tweenTime);
    }

    protected void RefreshButtonStatus(bool value)
    {
        _isOn = value;
        ToggleColorAndText(value);
        MoveIndicator(value);
    }

}
