using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance = null;
    public static SoundManager Instance { get => instance; }

    public static AudioClip audioClip;
    public static AudioSource audioSrc;

    public static AudioClip backgroundMusic;

    private static float backgroundMusicStartTime = 0f;

    //public AudioToggle switchForAudio;


    private float pitchStep = 0.01f;
    public float PitchStep { get => pitchStep; set => pitchStep = value; }
    //private float pitchStep = 0.00f;
    private float resetDelayTime = 0.5f;
    float defaultPitch;

    Coroutine coroutine, cooldownCoroutine;


    private bool isAudioSourceEnabled;
    public bool IsAudioSourceEnabled { get => isAudioSourceEnabled; }

    //private bool stackingSoundPlayedInThisFrame = false;
    //private float delayBetweenStartingOfStackingSounds = 0.033f, timeToResumePlayingTheStackSound=0f;
    //private float stackingSoundWCooldownVolume = 0.5f;


    public AudioSource defaultAudioSrc;
    public AudioSource backgroundMusicAudioSrc;

    //private MusicToggle musicToggle;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        audioSrc = defaultAudioSrc;
        defaultPitch = audioSrc.pitch;
        isAudioSourceEnabled = audioSrc.enabled;
    }

    /*     private void Update()
        {
            if (stackingSoundPlayedInThisFrame)
            {
                if(Time.time> timeToResumePlayingTheStackSound) stackingSoundPlayedInThisFrame = false;
            }

          //  Debug.LogError(backgroundMusicAudioSrc.time);
        } */
    // Start is called before the first frame update
    void Start()
    {

        /* stackingSound = Resources.Load<AudioClip>("stackingSound");
        fridgeOpen= Resources.Load<AudioClip>("fridgeOpen");
        fridgeClose = Resources.Load<AudioClip>("fridgeClose");
        percentInc = Resources.Load<AudioClip>("percentInc");
        giftAppears = Resources.Load<AudioClip>("giftAppears");
        stackingSoundAutoFill = Resources.Load<AudioClip>("stackingSoundAutoFill");
        coinClaim = Resources.Load<AudioClip>("coinClaim");
        compartmentOpen = Resources.Load<AudioClip>("compartmentOpen");
        compartmentClose= Resources.Load<AudioClip>("compartmentClose");
        chaChing= Resources.Load<AudioClip>("chaChing");
        endOfLevelJingle= Resources.Load<AudioClip>("endOfLevel");
        bubble= Resources.Load<AudioClip>("bubble");

        switchForAudio = AudioToggle.Instance; */

        //audioSrc.enabled = PlayerPrefs.GetInt("audio", -1) != 0;    //its false only if its 0 



        /*  if (switchForAudio)
         {
          //   switchForAudio.RefreshButtonStatus(audioSrc.enabled);
             switchForAudio.valueChanged += SwitchForAudio_valueChanged;
         }

         musicToggle = MusicToggle.Instance;
         backgroundMusicAudioSrc.enabled = PlayerPrefs.GetInt("music", -1) != 0;

         if (musicToggle)
         {
             musicToggle.valueChanged += MusicToggle_valueChanged;
         } */

        // backgroundMusic = Resources.Load<AudioClip>("backgroundMusic");

        backgroundMusicAudioSrc.loop = true;
        //  backgroundMusicAudioSrc.clip = backgroundMusic;
        //backgroundMusicAudioSrc.volume = 0.1f;
        //backgroundMusicAudioSrc.time = backgroundMusicStartTime;
        // if(backgroundMusicAudioSrc.enabled) backgroundMusicAudioSrc.Play(); 

        //Debug.LogError(backgroundMusicStartTime);

    }

    private void OnDisable()
    {
        if (backgroundMusicAudioSrc.enabled) backgroundMusicStartTime = backgroundMusicAudioSrc.time;
    }

    private void OnDestroy()
    {
        /*         if (switchForAudio) switchForAudio.valueChanged -= SwitchForAudio_valueChanged;
                if (musicToggle) musicToggle.valueChanged -= MusicToggle_valueChanged; */

    }

    private void SwitchForAudio_valueChanged(bool value)
    {
        audioSrc.enabled = value;
    }

    private void MusicToggle_valueChanged(bool value)
    {


        if (backgroundMusicAudioSrc.enabled && !value)
        {
            backgroundMusicStartTime = backgroundMusicAudioSrc.time;
            backgroundMusicAudioSrc.Stop();
        }

        if (!backgroundMusicAudioSrc.enabled && value)
        {
            backgroundMusicAudioSrc.time = backgroundMusicStartTime;
        }
        backgroundMusicAudioSrc.enabled = value;

        if (!backgroundMusicAudioSrc.enabled && value)
        {
            backgroundMusicAudioSrc.Play();
        }
    }



    public void PlaySound(string clip)
    {
        if (!audioSrc.enabled) return;
        /* switch (clip)
         {
             case "stackingSoundAutoFill":
                 //  if (audioSrc.isPlaying) return;
                 if (stackingSoundPlayedInThisFrame) return;
                 audioSrc.PlayOneShot(stackingSoundAutoFill, stackingSoundWCooldownVolume);
                /* audioSrc.pitch += PitchStep;
                 if (coroutine != null)
                 {
                     StopCoroutine(coroutine);
                 }
                 coroutine = StartCoroutine(ResetPitchCoroutine());
                 //
                 stackingSoundPlayedInThisFrame = true;
                 timeToResumePlayingTheStackSound = Time.time + delayBetweenStartingOfStackingSounds;
                 break;
             case "stackingSound":
                 //  if (audioSrc.isPlaying) return;
                // if (stackingSoundPlayedInThisFrame) return;
                 audioSrc.PlayOneShot(stackingSound);
                 audioSrc.pitch += PitchStep;
                 if (coroutine != null)
                 {
                     StopCoroutine(coroutine);
                 }
                 coroutine = StartCoroutine(ResetPitchCoroutine());
                 //
               //  stackingSoundPlayedInThisFrame = true;
                // timeToResumePlayingTheStackSound = Time.time + delayBetweenStartingOfStackingSounds;
                 break;
             case "fridgeOpen":
                 //  if (audioSrc.isPlaying) return;
                 audioSrc.PlayOneShot(fridgeOpen);
                 break;
             case "fridgeClose":
                 //  if (audioSrc.isPlaying) return;
                 audioSrc.PlayOneShot(fridgeClose);
                 break;
             case "percentInc":
                 audioSrc.PlayOneShot(percentInc,0.7f);
                 break;
             case "giftAppears":
                 audioSrc.PlayOneShot(giftAppears,0.3f);                
                 break;
             case "coinClaim":
                 if (cooldownCoroutine == null)
                 {
                     cooldownCoroutine= StartCoroutine(PlayOneShotWCooldown(audioSrc, coinClaim,1f,0.05f));   //cooldownCoroutine=null at the end of the Coroutine
                 }
                 break;
             case "compartmentOpen":
                 audioSrc.PlayOneShot(compartmentOpen,0.8f);
                 break;
             case "compartmentClose":
                 audioSrc.PlayOneShot(compartmentClose, 0.8f);
                 break;
             case "buttonPress":
                 audioSrc.PlayOneShot(stackingSoundAutoFill);
                 break;
             case "chaChing":
                 audioSrc.PlayOneShot(chaChing,0.3f);
                 audioSrc.pitch = 1.3f;
                 if (coroutine != null)
                 {
                     StopCoroutine(coroutine);
                 }
                 coroutine = StartCoroutine(ResetPitchCoroutine());
                 break;
             case "endOfLevel":
                 audioSrc.PlayOneShot(endOfLevelJingle,0.25f);
                 break;
             case "bubble":

                     if (cooldownCoroutine == null)
                     {
                         cooldownCoroutine = StartCoroutine(PlayOneShotWCooldown(audioSrc, bubble, 0.5f, 0.08f));   //cooldownCoroutine=null at the end of the Coroutine
                     }
                    // audioSrc.PlayOneShot(bubble);

                 break;
         }*/

    }

    public void PlaySound(AudioClip audioClip, float volume = 1f)
    {
        audioSrc.PlayOneShot(audioClip, volume);

        // audioSrc.pitch += PitchStep;
        /*         if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                } */
        // coroutine = StartCoroutine(ResetPitchCoroutine());
    }

    public void SwitchBackgroundMusic(AudioClip audioClip, float volume = 0.1f)
    {
        backgroundMusicAudioSrc.clip = audioClip;
        
        //backgroundMusicAudioSrc.time = backgroundMusicStartTime;
        if (backgroundMusicAudioSrc.enabled) backgroundMusicAudioSrc.Play();
        backgroundMusicAudioSrc.volume = volume;
    }

    

    public void PlaySoundWPitchChange(AudioClip audioClip, float volume = 1f)
    {
        audioSrc.PlayOneShot(audioClip, volume);

        audioSrc.pitch += PitchStep;
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(ResetPitchCoroutine());
    }

    public void ResetPitchToDefault()
    {
        audioSrc.pitch = defaultPitch;
    }

    IEnumerator ResetPitchCoroutine()
    {
        yield return new WaitForSeconds(resetDelayTime);
        audioSrc.pitch = defaultPitch;
    }

    IEnumerator PlayOneShotWCooldown(AudioSource src, AudioClip clip, float volume = 1f, float coolDown = 0.1f)
    {
        src.PlayOneShot(clip, volume);
        yield return new WaitForSeconds(coolDown);
        cooldownCoroutine = null;
    }

    void Reset()
    {
        audioSrc = GetComponent<AudioSource>();
        if (audioSrc)
        {
            audioSrc.playOnAwake = false;
        }
    }
}
