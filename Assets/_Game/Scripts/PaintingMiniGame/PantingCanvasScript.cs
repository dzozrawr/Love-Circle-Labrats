using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PantingCanvasScript : MonoBehaviour
{
    public MeshRenderer meshRenderer=null;
    
    public void SetCanvasMaterial(Material mat){
        meshRenderer.material=mat;
    }
}
