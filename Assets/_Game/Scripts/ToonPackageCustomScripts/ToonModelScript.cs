using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ToonModelScript : MonoBehaviour
{
    public SkinnedMeshRenderer headSkinnedMeshRenderer = null;

    public abstract void SetHeadMainMaterial(Material mat);
}
