using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitManager : MonoBehaviour
{
    private static List<GameObject> instantiatedObjects = new List<GameObject>();
    private void Awake()
    {
        instantiatedObjects.Add(gameObject);
    }
    public static void DestroyAllInstances()
    {
        foreach (GameObject go in instantiatedObjects)
        {
            if(go!=null)
            Destroy(go);

        }
    }

    public static void SetParentToAllInstances(Transform parent)
    {
        foreach (GameObject go in instantiatedObjects)
        {
            if(go!=null)
            go.transform.SetParent(parent);

        }
    }
}
