using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisionTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // gameObject.activeSelf;
    }


    private void OnParticleCollision(GameObject other) {
          Destroy(other);
    }
    private void OnCollisionEnter(Collision other)
    {
        //Debug.Log(other);
/*         foreach (Component item in other.transform.GetComponents<MonoBehaviour>())
        {
            
        } */
    
    }

}
