using System.Collections;
using UnityEngine;
using Crosstales.RTVoice;
using Crosstales.RTVoice.Model;

namespace PixelCrushers.DialogueSystem
{

    [AddComponentMenu("Dialogue System/Third Party Support/RT-Voice/RT Voice Actor")]
    public class RTVoiceActor : MonoBehaviour
    {

        public enum Gender { Female, Male, Any }

        [System.Serializable]
        public class VoicePreference
        {

            [Tooltip("The language code, such as 'es' for Spanish. Leave blank to match any language")]
            public string language;

            [Tooltip("The voice name to match. Leave blank to match any voice name. This Name can be a substring of the actual voice name")]
            public string name;

            [Tooltip("The gender to match")]
            public Gender gender;

            [Tooltip("The minimum age to match")]
            public int minAge = 0;

            [Tooltip("The maximum age to match")]
            public int maxAge = 100;
        }

        /// <summary>
        /// The voice preferences to try to match.
        /// </summary>
        [Tooltip("The voices preferences to match when speaking a line")]
        public VoicePreference[] voicePreferences = new VoicePreference[0];

        public float rate = 1;

        public float pitch = 1;

        public float volume = 1;

        /// <summary>
        /// In conversations, make the dialogue entry wait until RT-Voice is done playing.
        /// </summary>
        [Tooltip("In conversations, make the dialogue entry wait until RT-Voice is done playing")]
        public bool waitForVoiceInConversations = true;

        public enum VoiceState { Silent, Starting, Speaking }

        public VoiceState voiceState = VoiceState.Silent;

        protected AudioSource m_audioSource = null;

        protected bool m_started = false;

        public virtual void Awake()
        {
#if USE_SALSA
            var salsa = GetComponentInChildren<CrazyMinnow.SALSA.Salsa>();
            if (salsa != null) m_audioSource = salsa.audioSrc;
#endif
            if (m_audioSource == null)
            {
                m_audioSource = GetComponentInChildren<AudioSource>();
                if (m_audioSource == null) m_audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        public virtual void Start()
        {
            m_started = true;
            RegisterRTVoiceCallbacks();
        }

        public virtual void OnEnable()
        {
            RegisterRTVoiceCallbacks();
        }

        public virtual void OnDisable()
        {
            UnregisterRTVoiceCallbacks();
        }

        protected virtual void RegisterRTVoiceCallbacks()
        {
            if (!m_started) return;
            UnregisterRTVoiceCallbacks();
            Speaker.Instance.OnSpeakStart += OnSpeakStart;
            Speaker.Instance.OnSpeakComplete += OnSpeakComplete;
        }

        protected virtual void UnregisterRTVoiceCallbacks()
        {
            Speaker.Instance.OnSpeakStart -= OnSpeakStart;
            Speaker.Instance.OnSpeakComplete -= OnSpeakComplete;
        }

        protected virtual void OnSpeakStart(Crosstales.RTVoice.Model.Wrapper wrapper)
        {
            voiceState = VoiceState.Speaking;
        }

        protected virtual void OnSpeakComplete(Crosstales.RTVoice.Model.Wrapper wrapper)
        {
            voiceState = VoiceState.Silent;
        }

        /// <summary>
        /// When a conversation line is spoken, speak it through RT-Voice.
        /// </summary>
        /// <param name="subtitle">Subtitle.</param>
        public void OnConversationLine(Subtitle subtitle)
        {
            SpeakSubtitle(subtitle, waitForVoiceInConversations);
        }

        /// <summary>
        /// When a bark line is spoken, speak it through RT-Voice.
        /// </summary>
        /// <param name="subtitle">Subtitle.</param>
        public void OnBarkLine(Subtitle subtitle)
        {
            SpeakSubtitle(subtitle, false);
        }

        public void SpeakSubtitle(Subtitle subtitle, bool wait)
        {
            if (subtitle.speakerInfo.transform == this.transform)
            {
                var text = Tools.StripRichTextCodes(subtitle.formattedText.text);
                if (!string.IsNullOrEmpty(text))
                {
                    Speak(text);
                    if (wait)
                    {
                        var sequencer = DialogueManager.Instance.GetComponent<Sequencer>();
                        if (sequencer != null)
                        {
                            sequencer.PlayCommand("RTVoiceWait", false, 0, null, null);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Speak the specified text using RT-Voice. You can override this
        /// if you need to do anything extra.
        /// </summary>
        /// <param name="text">Text.</param>
        public virtual void Speak(string text)
        {
            if (!Speaker.Instance.areVoicesReady)
            {
                StartCoroutine(SpeakWhenVoicesAreReady(text));
            }
            else
            {
                var voice = GetVoice();
                var voiceName = (voice == null) ? "Default Speaker" : voice.Name;
                if (DialogueDebug.LogInfo) Debug.Log("Dialogue System: RT-Voice speaking with " + voiceName + ": '" + text + "'", this);
                Speaker.Instance.Silence();
                m_audioSource.Stop();
                voiceState = VoiceState.Starting;
                Speaker.Instance.Speak(text, m_audioSource, voice, true, rate, pitch, volume);
            }
        }

        protected virtual IEnumerator SpeakWhenVoicesAreReady(string text)
        {
            const float VoicesReadyTimeoutDuration = 5f;
            float timeout = Time.time + VoicesReadyTimeoutDuration;
            while (!Speaker.Instance.areVoicesReady && Time.time < timeout)
            {
                yield return null;
            }
            if (Speaker.Instance.areVoicesReady) Speak(text);
        }

        /// <summary>
        /// Finds the system's closest match voice to the preferred voice for the current
        /// language, or for the system language if no current language is set.
        /// </summary>
        /// <returns>The voice that best matches the actor's preferences.</returns>
        public Voice GetVoice()
        {
            var culture = Localization.Language;
           
            if (string.IsNullOrEmpty(culture)) culture = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
            var availableVoices = string.IsNullOrEmpty(culture) ? Speaker.Instance.Voices : Speaker.Instance.VoicesForCulture(culture);

            // Look for a voice that matches the current language:
            if (!string.IsNullOrEmpty(culture))
            {
                foreach (var voicePreference in voicePreferences)
                {
                    if (string.Equals(voicePreference.language, culture))
                    {
                        foreach (var availableVoice in availableVoices)
                        {
                            if (MatchesVoicePreference(availableVoice, voicePreference))
                            {
                                return availableVoice;
                            }
                        }
                    }
                }
            }

            // Failing that, look for a voice that matches the preferences regardless of language:
            foreach (var voicePreference in voicePreferences)
            {
                foreach (var availableVoice in Speaker.Instance.Voices)
                {
                   // Debug.LogError(availableVoice.Name+" age:"+typeof(availableVoice.Age));
                    if (MatchesVoicePreference(availableVoice, voicePreference))
                    {
                        return availableVoice;
                    }
                }
            }

            // Failing that, return the first voice in the list:
            return (availableVoices != null && availableVoices.Count > 0) ? availableVoices[0] : null;
        }

        protected bool MatchesVoicePreference(Voice voice, VoicePreference voicePreference)
        {
            var gender = (voice.Gender == Crosstales.RTVoice.Model.Enum.Gender.FEMALE) ? Gender.Female
                : (voice.Gender == Crosstales.RTVoice.Model.Enum.Gender.FEMALE) ? Gender.Male
                : Gender.Any;
            var age = Tools.StringToInt(voice.Age);
            var matchesName = string.IsNullOrEmpty(voicePreference.name) ||
                (!string.IsNullOrEmpty(voice.Name) && voice.Name.Contains(voicePreference.name));
            if(matchesName) return matchesName; //MY ADDITION
            var matchesGender = (voicePreference.gender == Gender.Any) || (gender == voicePreference.gender);
            if (matchesGender) return matchesGender;    //MY ADDITION
            var matchesAge = (voicePreference.minAge <= age && age <= voicePreference.maxAge);
            //return matchesName && matchesGender && matchesAge; //THE ORIGINAL CODE
            //return matchesName && matchesGender;
            return matchesAge;
        }

    }

}
