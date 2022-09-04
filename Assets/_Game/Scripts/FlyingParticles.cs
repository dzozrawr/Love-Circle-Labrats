using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEngine.ParticleSystem;
using UnityEngine.Events;

// [RequireComponent(typeof(ParticleSystem))]
public class FlyingParticles : MonoBehaviour
{
    [Header("Projectile")]
    public Transform target;
    public float projectileSpeed = 10.0f;
    public ParticleSystem particles;

    public UnityEvent onCoinsReachedTarget;
    public UnityEvent onCoinReachedTarget;

    Action updater;
    static Particle[] buffer = new Particle[32];

    public float acceptableDistanceFromTarget = 4f;
    public virtual void Shoot(Transform target)
    {
        particles.Play();
        StartCoroutine(ShootDelayed(target));
    }

    public virtual void Shoot()
    {
        particles.Play();
        StartCoroutine(ShootDelayed(target));
    }

    protected IEnumerator ShootDelayed(Transform target, float delay = 0.75f)
    {
        if (delay > 0.0f)
        {
            yield return new WaitForSeconds(delay);
        }
        this.target = target;
        updater = MoveParticles;
       // particles.Stop();
    }

    void MoveParticles()
    {
        if (target != null)
        {
            int count = particles.GetParticles(buffer);

            if (count == 0)
            {
                onCoinsReachedTarget.Invoke();
                OnReachedTarget();
                target = null;
                updater = null;
                particles.Stop();
                return;
            }

            for (int i = 0; i < count; i++)
            {
                //Vector3 direction = new Vector3(target.position.x - buffer[i].position.x, target.position.y - buffer[i].position.y, buffer[i].position.z); 
                Vector3 direction = target.position - buffer[i].position;

                // direction = new Vector3(direction.x, direction.y, buffer[i].position.z);
               // Debug.LogError(Vector3.Distance(target.position, buffer[0].position));
                if (Vector3.Distance(target.position, buffer[i].position) <= acceptableDistanceFromTarget)
                {
                  //  buffer[i].position = target.position;
                    
                    //StartCoroutine(KillParticle(buffer[i]));
                    buffer[i].remainingLifetime = 0f;
                    onCoinReachedTarget?.Invoke();
                    continue;
                }
                buffer[i].position = buffer[i].position + direction.normalized * projectileSpeed * Time.deltaTime;
               // Debug.LogError(buffer[0].position);
            }

            particles.SetParticles(buffer, count);
        }
    }

    IEnumerator KillParticle(Particle particle)
    {
        yield return new WaitForSeconds(0.2f);
        particle.remainingLifetime = -1.0f;
        Debug.LogError("particle.remainingLifetime = -1.0f;");
    }

    public void KillParticle()
    {

    }

    public virtual void OnReachedTarget()
    {

    }

    void Update()
    {
        updater?.Invoke();
    }

    void Reset()
    {
        particles = GetComponent<ParticleSystem>();
    }
}
