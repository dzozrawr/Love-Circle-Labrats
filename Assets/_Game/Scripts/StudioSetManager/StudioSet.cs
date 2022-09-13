using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Contestant;

public abstract class StudioSet : MonoBehaviour
{
    public Text episodeNumberText=null;
   // public PlayerScript playerL = null, playerR = null;

    public Material skybox;

    public AudioClip startingMusic = null;
    [Range(0f, 1f)]
    public float startingMusicVolume = 0.1f;
    public AudioClip questioningMusic = null;
    [Range(0f, 1f)]
    public float questioningMusicVolume = 0.1f;
    public AudioClip finalMusic = null;
    [Range(0f, 1f)]
    public float finalMusicVolume = 0.1f;


    public AudioClip eliminationAudioClip = null;
    [Range(0f, 1f)]
    public float eliminationAudioClipVolume = 1f;


    protected ContestantQuestioningManager contestantQuestioningManager = null;
    protected virtual void Start()
    {
        contestantQuestioningManager = ContestantQuestioningManager.Instance;
        episodeNumberText.text="Episode "+SceneManager.GetActiveScene().buildIndex;        
    }

    protected virtual void OnEnable()
    {
        RenderSettings.skybox = skybox;
        if (startingMusic != null)
            SoundManager.Instance.SwitchBackgroundMusic(startingMusic, startingMusicVolume);
    }
    public abstract void OpenPlayerCurtain(PlayerScript player);
    public abstract void EliminateContestant(ContestantScript contestant);
}
