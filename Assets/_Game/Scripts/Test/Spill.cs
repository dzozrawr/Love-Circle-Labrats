using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class Spill : MonoBehaviour
{
    ParticleSystem particleSystem;
    public float startAngle = 120f;
    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Vector3.Angle(Vector3.down, transform.forward) <= startAngle))
        {
            particleSystem.Play();
        }
        else
        {
            particleSystem.Stop();
        }
    }
}
