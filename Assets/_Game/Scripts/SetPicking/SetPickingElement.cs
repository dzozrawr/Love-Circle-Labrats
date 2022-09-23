using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NiceVibrations.CrazyLabsExtension;
using TMPro;

public class SetPickingElement : MonoBehaviour
{
    //public Sprite selectedSprite = null;
    //public Sprite unselectedSprite = null;
    public SetShop.SetID id;
    public StudioSet studioSet = null;

    public GameObject setSelectedBackgroundGO = null;

    public GameObject priceGameObject = null;

    private int price = 0;

    private GameController gameController = null;

    private SetPickingElement[] setPickingElements;


    private void Awake()
    {
        price = int.Parse(priceGameObject.GetComponentInChildren<TMP_Text>().text);
    }


    private void Start()
    {
        gameController = GameController.Instance;
        if (gameController.studioSet == studioSet) setSelectedBackgroundGO.SetActive(true);
        setPickingElements = transform.parent.GetComponentsInChildren<SetPickingElement>();
    }

    // Update is called once per frame
    public void OnButtonClick()
    {
        if (priceGameObject.activeSelf)
        {
            if (GameController.CoinAmount >= price)
            {
                ChooseSet();

                GameController.Instance.SetCoinAmount(GameController.CoinAmount-price);
                priceGameObject.SetActive(false);
            }
        }
        else
        {
           ChooseSet();
        }
    }

    private void ChooseSet()
    {
        setSelectedBackgroundGO.SetActive(true);


        for (int i = 0; i < setPickingElements.Length; i++)
        {
            if (setPickingElements[i] == this) continue;
            setPickingElements[i].setSelectedBackgroundGO.SetActive(false);

            //setPickingElements[i].image.sprite = setPickingElements[i].unselectedSprite;
        }

        gameController.PickSet(studioSet.gameObject);

        //if(HapticFeedbackController.)
        HapticFeedbackController.TriggerHaptics(MoreMountains.NiceVibrations.HapticTypes.Selection);
    }
}
