using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentOnCollision : MonoBehaviour
{
    public GameObject gameObjectToParent=null;
    public string tagToIgnore;
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag.Equals(tagToIgnore)) return;
        if(gameObjectToParent!=null)gameObjectToParent.transform.SetParent(other.gameObject.transform); else transform.SetParent(other.gameObject.transform);        
    }
}
