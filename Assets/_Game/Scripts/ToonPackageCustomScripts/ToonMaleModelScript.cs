using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToonMaleModelScript : ToonModelScript
{
    public int indOfMainMaterial = -1;
    public override void SetHeadMainMaterial(Material mat)
    {
        if(indOfMainMaterial==-1)
        headSkinnedMeshRenderer.materials = new Material[3] { mat, headSkinnedMeshRenderer.materials[1], headSkinnedMeshRenderer.materials[2] };
        else
        {
            Material[] materials = new Material[3];
            for (int i = 0; i < materials.Length; i++)
            {
                if (indOfMainMaterial == i)
                {
                    materials[i] = mat;
                }
                else
                {
                    materials[i] = headSkinnedMeshRenderer.materials[i];
                }
            }

            headSkinnedMeshRenderer.materials = materials;
        }
    }
}
