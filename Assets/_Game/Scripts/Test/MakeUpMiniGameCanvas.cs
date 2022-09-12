using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaintIn3D;
using UnityEngine.UI;

public class MakeUpMiniGameCanvas : MonoBehaviour
{
    public P3dChannelCounter channelCounter = null;
    
    [Range(0,2432)]
    public int redCounterFinishValue = 2368;

    public Slider progressBarSlider = null;
    // Start is called before the first frame update
    void Start()
    {
        progressBarSlider.value = 0;
        progressBarSlider.maxValue = redCounterFinishValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            progressBarSlider.value = channelCounter.CountR;
        }
        
    }
}
