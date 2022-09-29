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
    public GameObject lockOverlay = null;

    public int price = 0;

    private GameController gameController = null;

    private SetPickingElement[] setPickingElements;

    private bool isBought = false;

    private TMP_Text priceText = null;

    public bool IsBought { get => isBought; set => isBought = value; }

    // public int Price { get => price; set => price = value; }

    private void Awake()
    {
        isBought = SetShop.IsSetBought(id);

        if(id==SetShop.SetID.None){
            SetLockedAppearance(false);
        }else
        SetLockedAppearance(!isBought);

        priceText = priceGameObject.GetComponentInChildren<TMP_Text>();
        priceText.text = price + "";
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

                GameController.Instance.SetCoinAmount(GameController.CoinAmount - price);

                SetLockedAppearance(false);

                isBought = false;

                SetShop.Instance.SetSetAsBought(id);

                SaveSystem.SaveGameAsync(new SaveData());
            }
        }
        else
        {
            ChooseSet();
        }
    }

    private void SetLockedAppearance(bool isLocked)
    {
        priceGameObject.SetActive(isLocked);
        lockOverlay.SetActive(isLocked);
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


        HapticFeedbackController.TriggerHaptics(MoreMountains.NiceVibrations.HapticTypes.Selection);
    }
}
