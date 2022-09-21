using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActorScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        SoundManager.Instance.AudioSourcesOfVoices.Add(audioSource);

        if (PlayerPrefs.GetInt("voices", -1) != 0)
        {
            audioSource.volume = 1f;
        }
        else
        {
            audioSource.volume = 0f;
        }

    }


}
