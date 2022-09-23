using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NiceVibrations.CrazyLabsExtension;

public class SetPickingElement : MonoBehaviour
{
    //public Sprite selectedSprite = null;
    //public Sprite unselectedSprite = null;
    public StudioSet studioSet = null;

    public GameObject setSelectedBackgroundGO = null;

    private GameController gameController = null;

    private SetPickingElement[] setPickingElements;




    private void Start()
    {
        gameController = GameController.Instance;
        if (gameController.studioSet == studioSet) setSelectedBackgroundGO.SetActive(true);
        setPickingElements = transform.parent.GetComponentsInChildren<SetPickingElement>();
    }

    // Update is called once per frame
    public void OnButtonClick()
    {
        setSelectedBackgroundGO.SetActive(true);

        for (int i = 0; i < setPickingElements.Length; i++)
        {
            if (setPickingElements[i] == this) continue;
            setPickingElements[i].setSelectedBackgroundGO.SetActive(false);

            //setPickingElements[i].image.sprite = setPickingElements[i].unselectedSprite;
        }

        gameController.PickSet(studioSet.gameObject);

        HapticFeedbackController.TriggerHaptics(MoreMountains.NiceVibrations.HapticTypes.Selection);
    }
}
