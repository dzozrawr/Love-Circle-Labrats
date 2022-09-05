using MoreMountains.NiceVibrations;
using UnityEngine;
using UnityEngine.UI;

namespace NiceVibrations.CrazyLabsExtension
{
    [RequireComponent( typeof( Button ) )]
    public class HapticFeedbackUIButton : MonoBehaviour
    {
        private Button _buttonCached;

        [SerializeField]
        private HapticTypes _hapticType = HapticTypes.Selection;

        private void Awake()
        {
            _buttonCached = GetComponent<Button>( );

            _buttonCached.onClick.AddListener( TriggerHaptics );
        }

        private void OnDestroy()
        {
            _buttonCached.onClick.RemoveListener( TriggerHaptics );
        }

        private void TriggerHaptics()
        {
            HapticFeedbackController.TriggerHaptics( _hapticType, true );
        }
    }
}
