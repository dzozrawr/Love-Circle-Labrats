using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SugarProgressBar : MonoBehaviour
{
    public Slider slider = null;
    // Start is called before the first frame update
    void Start()
    {
        slider.value = 0;
    }

    public void SetFill(float fill)
    {
        slider.value = fill;
    }
}
