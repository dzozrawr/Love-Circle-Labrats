using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class Spill : MonoBehaviour
{
    ParticleSystem particleSystem;
    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Vector3.Angle(Vector3.down, transform.forward) <= 90f))
        {
            particleSystem.Play();
        }
        else
        {
            particleSystem.Stop();
        }
    }
}
