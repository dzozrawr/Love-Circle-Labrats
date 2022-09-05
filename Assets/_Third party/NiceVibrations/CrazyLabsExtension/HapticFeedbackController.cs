using System;
using MoreMountains.NiceVibrations;
using UnityEngine;

namespace NiceVibrations.CrazyLabsExtension
{
    public class HapticFeedbackController : MonoBehaviour
    {
        public static event Action<bool> OnHapticsEnabledChanged = delegate{ };

        private static HapticFeedbackController _instance;

        private float _hapticTimer     = 0f;
        private bool  _currentlyActive = false;
        private bool  _hapticsPaused   = false;

        //private const float  _hapticMinimumDelay        = 0.1f;
        private const float _hapticMinimumDelay = 0.05f;
        private const string _hapticsEnabledPrefsString = "HapticsEnabled";

        private void Awake()
        {
       
            if (_instance == null)   //ADDED BY ME
            {
                DontDestroyOnLoad(this);   //ADDED BY ME                
            }
            else
            {
                Destroy(this.gameObject);   //ADDED BY ME
                return; //ADDED BY ME
            }

            _instance        = this;
            _currentlyActive = PlayerPrefs.GetInt( _hapticsEnabledPrefsString, 1 ) == 1;

#if TTP_CORE
            Tabtale.TTPlugins.TTPCore.PauseGameMusicEvent += PauseHapticsDuringAds;
#endif
#if UNITY_IOS
            MMVibrationManager.iOSInitializeHaptics();
#endif
        }

        private void OnDestroy()
        {
#if TTP_CORE
            Tabtale.TTPlugins.TTPCore.PauseGameMusicEvent -= PauseHapticsDuringAds;
#endif
        }

        private void Update()
        {
            _hapticTimer -= Time.deltaTime;
        }

        private void PauseHapticsDuringAds( bool shouldPause )
        {
            _hapticsPaused = shouldPause;
        }

        public static bool IsHapticsEnabled => _instance._currentlyActive;

        public static void ToggleHaptics( bool isOn )
        {
            _instance._currentlyActive = isOn;

            PlayerPrefs.SetInt( _hapticsEnabledPrefsString, _instance._currentlyActive ? 1 : 0 );

            OnHapticsEnabledChanged.Invoke( _instance._currentlyActive );
        }

        public static void ToggleHaptics()
        {
            _instance._currentlyActive = !_instance._currentlyActive;

            PlayerPrefs.SetInt( _hapticsEnabledPrefsString, _instance._currentlyActive ? 1 : 0 );

            OnHapticsEnabledChanged.Invoke( _instance._currentlyActive );
        }

        public static void TriggerHaptics( HapticTypes type, bool force = false )
        {
            if ( _instance._hapticsPaused )
                return;

            if ( !_instance._currentlyActive )
                return;

            if ( _instance._hapticTimer > 0f && !force )
                return;

            MMVibrationManager.Haptic( type );
            _instance._hapticTimer = _hapticMinimumDelay;
        }
    }
}
