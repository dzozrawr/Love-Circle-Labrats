using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NiceVibrations.CrazyLabsExtension;

[RequireComponent(typeof(Image))]
public class SetPickingElement : MonoBehaviour
{
    public Sprite selectedSprite = null;
    public Sprite unselectedSprite = null;
    public StudioSet studioSet = null;

    private GameController gameController = null;
    private Image image = null;
    private SetPickingElement[] setPickingElements;


    private void Awake()
    {
        image = GetComponent<Image>();
    }


    private void Start()
    {
        gameController = GameController.Instance;
        if (gameController.studioSet == studioSet) image.sprite = selectedSprite;
        setPickingElements = transform.parent.GetComponentsInChildren<SetPickingElement>();
    }

    // Update is called once per frame
    public void OnButtonClick()
    {
        image.sprite = selectedSprite;

        for (int i = 0; i < setPickingElements.Length; i++)
        {
            if (setPickingElements[i] == this) continue;
            setPickingElements[i].image.sprite = setPickingElements[i].unselectedSprite;
        }

        gameController.PickSet(studioSet.gameObject);

        HapticFeedbackController.TriggerHaptics(MoreMountains.NiceVibrations.HapticTypes.Selection);
    }
}
