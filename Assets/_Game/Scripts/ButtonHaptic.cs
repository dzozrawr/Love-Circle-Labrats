using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NiceVibrations.CrazyLabsExtension;
using UnityEngine.UI;

public class ButtonHaptic : MonoBehaviour
{
    public bool shouldAddAsNonPermanentListener=false;

    private void Start() {
        if(shouldAddAsNonPermanentListener){
            GetComponent<Button>().onClick.AddListener(OnClick);
        }
    }
    public void OnClick(){
        HapticFeedbackController.TriggerHaptics(MoreMountains.NiceVibrations.HapticTypes.Selection);
    }
}
