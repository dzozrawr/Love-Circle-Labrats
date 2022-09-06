using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFruitOnTriggerCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        FruitManager fm=other.GetComponentInParent<FruitManager>();
        if(fm!=null){
            Destroy(fm.gameObject);
        }
    }
}
