using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
using Crosstales.RTVoice;

namespace PixelCrushers.DialogueSystem.SequencerCommands
{

    /// <summary>
    /// Sequencer command RTVoiceWait().
    /// Waits for the current RT-Voice audio clip to finish, or a hard limit of
    /// 60 seconds for the voice to start.
    /// 
    /// If you're using SALSA LipSync Suite and define the scripting symbol 
    /// USE_SALSA, it will monitor the SALSA audio source to know when the
    /// TTS is done.
    /// </summary>
    public class SequencerCommandRTVoiceWait : SequencerCommand
    {

        private const float MaxSecondsWaitToStart = 60;
        private AudioSource speakerAudioSource = null;
        private AudioClip audioClip = null;

        public IEnumerator Start()
        {
            var subject = GetSubject(0, Sequencer.Speaker);

#if USE_SALSA
            var salsa = GetComponentInChildren<CrazyMinnow.SALSA.Salsa>();
            if (salsa != null) speakerAudioSource = salsa.audioSrc;
#endif

            if (speakerAudioSource == null) speakerAudioSource = (subject == null) ? null : subject.GetComponentInChildren<AudioSource>();
            var rtVoiceActor = (subject == null) ? null : subject.GetComponent<RTVoiceActor>();

            if (speakerAudioSource == null)
            {
                if (DialogueDebug.LogWarnings) Debug.LogWarning("Dialogue System: RTVoiceWait(): Can't find audio source on " + GetParameter(0) + " speaker=" + subject + ".");
            }
            if (rtVoiceActor == null)
            {
                if (DialogueDebug.LogWarnings) Debug.LogWarning("Dialogue System: RTVoiceWait(): Can't find RTVoiceActor on " + GetParameter(0) + " speaker=" + subject + ". Not waiting for RT-Voice to finish.");
            }
            else
            {
                if (DialogueDebug.LogInfo) Debug.Log("Dialogue System: Sequencer: RTVoiceWait() on " + subject.name, subject);

                // Wait for audio source to start playing:
                var timeout = Time.time + MaxSecondsWaitToStart;
                while ((rtVoiceActor.voiceState != RTVoiceActor.VoiceState.Speaking) && (Time.time < timeout))
                {
                    yield return null;
                }

                if (Time.time >= timeout)
                {
                    if (DialogueDebug.LogWarnings) Debug.LogWarning("Dialogue System: RTVoiceWait(): Timed out waiting for RT-Voice to start playing on " + subject);
                }
                else
                {
                    audioClip = (speakerAudioSource == null) ? null : speakerAudioSource.clip;
                    while ((rtVoiceActor.voiceState != RTVoiceActor.VoiceState.Silent) && (Time.time < timeout))
                    {
                        yield return null;
                    }
                    if (Time.time >= timeout)
                    {
                        if (DialogueDebug.LogWarnings) Debug.LogWarning("Dialogue System: RTVoiceWait(): Timed out waiting for RT-Voice to stop playing on " + subject);
                    }
                }
            }

            Stop();
        }

        public void OnDestroy()
        {
            if ((speakerAudioSource != null) && speakerAudioSource.isPlaying && (speakerAudioSource.clip == audioClip))
            {
                speakerAudioSource.Stop();
            }
        }

    }

}
