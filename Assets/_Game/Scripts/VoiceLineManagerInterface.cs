using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceLineManagerClass
{
    private AudioSource audioSource=null;

    private bool wasAudioSourcePlaying=false;

    private Animator animator=null;
    public VoiceLineManagerClass(AudioSource audioSource, Animator animator){
        this.audioSource=audioSource;
        this.animator=animator;
    }

    // Update is called once per frame
    public void DoUpdate()
    {
        if(!audioSource.isPlaying&&wasAudioSourcePlaying){
            animator.SetTrigger("Idle");
        }
        wasAudioSourcePlaying=audioSource.isPlaying;
    }
}
