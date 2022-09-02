using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickingTrigger : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        //show UI elements for player picking
        GameCanvasController.Instance.ShowPlayerPickingButtons();
    }
}
