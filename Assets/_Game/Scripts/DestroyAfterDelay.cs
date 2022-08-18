using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
    public float delay=1f;

    private float timer=0f;

    void Update()
    {
        timer+=Time.deltaTime;
        if(timer>=delay){
            Destroy(gameObject);
        }
    }
}
