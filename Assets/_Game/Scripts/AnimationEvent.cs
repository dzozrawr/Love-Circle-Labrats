using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public ParticleSystem heartPulseParticle = null;
    public ParticleSystem brokenHeartParticle = null;
    public ParticleSystem coinBurstParticle = null;
    public FlyingParticles flyingParticles = null;
    public void PlayHeartPulseParticle()
    {
        heartPulseParticle.Play();
    }

    public void PlayBrokenHeartParticle()
    {
        brokenHeartParticle.Play();
    }

    public void PlayCoinBurstParticle()
    {
        coinBurstParticle.Play();
        flyingParticles.Shoot();
    }
}
