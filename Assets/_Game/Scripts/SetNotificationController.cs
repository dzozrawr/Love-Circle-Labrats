using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetNotificationController : MonoBehaviour
{
    public TMP_Text notificationNumberTxt = null;
    public SetShop setShop = null;
    private SetPickingElement[] setPickingElements = null;
    private Image img = null;

    private void Awake()
    {
        img = GetComponent<Image>();
        ShowOrHide(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        GameController.Instance.CoinAmountUpdated.AddListener(RefreshNotificationStatus);

        setShop.SetBought.AddListener(RefreshNotificationStatus);

        //setPickingElements = GameObject.FindObjectsOfType<SetPickingElement>();
        setPickingElements = setShop.setsInShopInstances.ToArray();
        RefreshNotificationStatus();
    }

    private void RefreshNotificationStatus()
    {
        int notificationNumber = 0;

        if (SetShop.Instance == null)
        {
            for (int i = 0; i < setPickingElements.Length; i++)
            {
                if (setPickingElements[i].price > 0)
                {
                    if ((GameController.CoinAmount >= setPickingElements[i].price) && (!SetShop.IsSetBought(setPickingElements[i].id)))
                    {
                        notificationNumber++;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < setPickingElements.Length; i++)
            {
                if (setPickingElements[i].price > 0)
                {
                    if ((GameController.CoinAmount >= setPickingElements[i].price) && (!setPickingElements[i].IsBought))
                    {
                        notificationNumber++;
                    }
                }
            }
        }
        if (notificationNumber == 0)
        {
            ShowOrHide(false);
        }
        else
        {
            notificationNumberTxt.text = notificationNumber + "";
            ShowOrHide(true);
        }
    }

    public void ShowOrHide(bool show)
    {

        img.enabled = show;
        notificationNumberTxt.enabled = show;

    }
}
