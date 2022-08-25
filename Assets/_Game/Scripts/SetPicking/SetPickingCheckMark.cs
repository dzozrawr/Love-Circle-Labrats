using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPickingCheckMark : MonoBehaviour
{
    public void OnButtonClick()
    {
        GameController.Instance.ApplySelectedStudioSet();
    }
}
