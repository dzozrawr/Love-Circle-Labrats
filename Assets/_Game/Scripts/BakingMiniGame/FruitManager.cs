using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitManager : MonoBehaviour
{
    private static List<GameObject> instantiatedObjects=new List<GameObject>();
    private void Awake() {
        instantiatedObjects.Add(gameObject);
    }
    public static void DestroyAllInstances(){
        foreach (GameObject go in instantiatedObjects)
        {
            Debug.Log(go.name+" destroyed");
            Destroy(go);

        }
    }
}
