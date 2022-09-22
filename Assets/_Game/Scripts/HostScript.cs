using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class HostScript : MonoBehaviour
{
    private AudioSource audioSource;

    private bool wasAudioSourcePlaying = false;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.AudioSourcesOfVoices.Add(audioSource);

       // audioSource.enabled = (PlayerPrefs.GetInt("voices", -1) != 0);

        if (PlayerPrefs.GetInt("voices", -1) != 0)
        {
            audioSource.volume = 1f;
        }
        else
        {
            audioSource.volume = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if(audioSource.isPlaying)
        if (!audioSource.isPlaying && wasAudioSourcePlaying)
        {
            GetComponentInChildren<Animator>().SetTrigger("Idle");
        }
        wasAudioSourcePlaying = audioSource.isPlaying;
    }
}
