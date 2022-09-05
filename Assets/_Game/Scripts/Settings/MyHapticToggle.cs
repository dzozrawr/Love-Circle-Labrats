using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using NiceVibrations.CrazyLabsExtension;

public class MyHapticToggle : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private RectTransform toggleIndicator;

    private Image toggleIndicatorImage;

    [SerializeField]
    private Image backgroundImage;

    [SerializeField]
    private Color onColor;

    [SerializeField]
    private Color offColor;

    private float offX;
    private float onX;

    [SerializeField]
    private float tweenTime = 0.25f;

    private AudioSource audioSource;

    public delegate void ValueChanged(bool value);
    public event ValueChanged valueChanged;

    public Text onTxt, offTxt;

    public void OnPointerDown(PointerEventData eventData)
    {
        HapticFeedbackController.TriggerHaptics(MoreMountains.NiceVibrations.HapticTypes.Selection);  //play haptic
        Toggle(!HapticFeedbackController.IsHapticsEnabled);//flips the switch when clicked
    }

    // Start is called before the first frame update
    void Start()
    {
        toggleIndicatorImage = toggleIndicator.gameObject.GetComponent<Image>();

        offX = toggleIndicator.anchoredPosition.x;
        onX = backgroundImage.rectTransform.rect.width - toggleIndicator.rect.width;

        audioSource = this.GetComponent<AudioSource>();

        //HapticFeedbackController.ToggleHaptics();

        RefreshButtonStatus(HapticFeedbackController.IsHapticsEnabled);

    }

    private void RefreshButtonStatus(bool value)
    {
        ToggleColorAndText(value);
        MoveIndicator(value);
    }


    public void Toggle(bool value, bool playSFX = false)
    {
        if (value != HapticFeedbackController.IsHapticsEnabled)
        {
            HapticFeedbackController.ToggleHaptics();

            ToggleColorAndText(HapticFeedbackController.IsHapticsEnabled);
            MoveIndicator(HapticFeedbackController.IsHapticsEnabled);

            if (playSFX) audioSource.Play();

            //   if (valueChanged != null) valueChanged(isOn);
        }
    }

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
    /*    private void ToggleColor(bool value)
        {
            if (value) toggleIndicatorImage.DOColor(onColor, tweenTime);
            else
                toggleIndicatorImage.DOColor(offColor, tweenTime);
        }*/

    private void MoveIndicator(bool value)
    {
        if (value)
            toggleIndicator.DOAnchorPosX(onX, tweenTime);
        else
            toggleIndicator.DOAnchorPosX(offX, tweenTime);
    }

}
