using UnityEngine;
using UnityEngine.UI;

namespace NiceVibrations.CrazyLabsExtension
{
    [RequireComponent( typeof( Button ) )]
    public class HapticFeedbackToggle : MonoBehaviour
    {
        private Button _buttonCached;

        [SerializeField]
        private Image _activeImage;

        [SerializeField]
        private Sprite _hapticsActiveSprite;

        [SerializeField]
        private Sprite _hapticsInactiveSprite;

        private void Awake()
        {
            _buttonCached = GetComponent<Button>( );

            _buttonCached.onClick.AddListener( ToggleHaptics );

            HapticFeedbackController.OnHapticsEnabledChanged += RefreshButtonStatus;
        }

        private void OnDestroy()
        {
            _buttonCached.onClick.RemoveListener( ToggleHaptics );

            HapticFeedbackController.OnHapticsEnabledChanged -= RefreshButtonStatus;
        }

        private void Start()
        {
            RefreshButtonStatus( HapticFeedbackController.IsHapticsEnabled );
        }

        private void ToggleHaptics()
        {
            HapticFeedbackController.ToggleHaptics( );
        }

        private void RefreshButtonStatus( bool isHapticsActive )
        {
            _activeImage.sprite = isHapticsActive ? _hapticsActiveSprite : _hapticsInactiveSprite;
        }
    }
}
