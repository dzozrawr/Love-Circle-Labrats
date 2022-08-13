using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StudioSet : MonoBehaviour
{
    public PlayerScript playerL=null, playerR=null;

    public abstract void OpenPlayerCurtain(PlayerScript player);
}
