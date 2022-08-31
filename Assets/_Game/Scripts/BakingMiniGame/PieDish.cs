using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieDish : MonoBehaviour
{
    public GameObject unbakedBody=null;
    public GameObject topLayer=null;
    public Transform topLayerAbovePos=null;
    public Transform pieCutterAbovePos=null;
    
    public GameObject[] pieTopLayers;
    public GameObject[] pieCutters;
    public GameObject[] pieBakedRedTopLayers;
    public GameObject[] pieBakedBlueTopLayers;
    public GameObject bakedBody=null;
    public ParticleSystem goodBakedParticles=null;
    public ParticleSystem badBakedParticles=null;
}
