using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToonModelScript : MonoBehaviour
{
    public SkinnedMeshRenderer headSkinnedMeshRenderer = null;

    public void SetHeadMainMaterial(Material mat)
    {
        headSkinnedMeshRenderer.materials = new Material[4] { headSkinnedMeshRenderer.materials[0], mat, headSkinnedMeshRenderer.materials[2], headSkinnedMeshRenderer.materials[3] };
    }
}
