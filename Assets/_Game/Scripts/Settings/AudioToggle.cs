using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using NiceVibrations.CrazyLabsExtension;

public class AudioToggle : MonoBehaviour, IPointerDownHandler
{
    private static AudioToggle instance;
    public static AudioToggle Instance { get => instance; }

    [SerializeField] private bool _isOn = false;

    public bool isOn
    {
        get
        {
            return _isOn;
        }
    }



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





    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);    //this line of code shouldnt happen ever
            return;
        }
        instance = this;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        HapticFeedbackController.TriggerHaptics(MoreMountains.NiceVibrations.HapticTypes.Selection);  //play haptic
        Toggle(!isOn);//flips the switch when clicked
    }

    // Start is called before the first frame update
    void Start()
    {
        toggleIndicatorImage = toggleIndicator.gameObject.GetComponent<Image>();

        offX = toggleIndicator.anchoredPosition.x;
        onX = backgroundImage.rectTransform.rect.width - toggleIndicator.rect.width;

        audioSource = this.GetComponent<AudioSource>();

        int audioInt = PlayerPrefs.GetInt("audio", -1);
        if (audioInt == -1)
        {
            RefreshButtonStatus(true);
        }
        else
        {
            RefreshButtonStatus(audioInt == 1);
        }
        //  _isOn = SoundManager.Instance.IsAudioSourceEnabled;
        // RefreshButtonStatus(SoundManager.Instance.IsAudioSourceEnabled);
        ///Toggle(!isOn, false);
    }
    /*    RefreshButtonStatus(HapticFeedbackController.IsHapticsEnabled);

    }
    */
    public void RefreshButtonStatus(bool value)
    {
        _isOn = value;
        ToggleColorAndText(value);
        //toggleIndicator.anchoredPosition = new Vector2(value?onX:offX, toggleIndicator.anchoredPosition.y);
        MoveIndicator(value);
    }

    private void OnEnable()
    {
        // Toggle(isOn);
    }

    public void Toggle(bool value, bool playSFX = true)
    {
        if (value != isOn)
        {
            _isOn = value;

            PlayerPrefs.SetInt("audio", _isOn ? 1 : 0);
            ToggleColorAndText(isOn);
            MoveIndicator(isOn);

            if (playSFX) audioSource.Play();

            if (valueChanged != null) valueChanged(isOn);
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
