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
        gameObject.GetComponentInChildren<Animator>().SetTrigger("egg");
        GameObject eggYolkInstance= Instantiate(eggYolk, placeForYolk.transform.position, Quaternion.identity);
        eggYolkInstance.transform.SetParent(transform.parent);
        BakingMiniGame.Instance.EggYolks.Add(eggYolkInstance);
        //Debug.Log(transform.position);
    }
}
