using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenEgg : MonoBehaviour
{
    public GameObject eggYolk = null;
    public GameObject placeForYolk = null;

    public void BreakEgg()
    {
        //play animation of egg breaking
        GameObject eggYolkInstance= Instantiate(eggYolk, placeForYolk.transform.position, Quaternion.identity);
        eggYolkInstance.transform.SetParent(transform.parent);
        //Debug.Log(transform.position);
    }
}
