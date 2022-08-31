using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentOnCollision : MonoBehaviour
{
    public GameObject gameObjectToParent=null;
    private void OnCollisionEnter(Collision other) {
        if(gameObjectToParent!=null)gameObjectToParent.transform.SetParent(other.gameObject.transform); else transform.SetParent(other.gameObject.transform);        
    }
}
