using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EOLScreenController : MonoBehaviour
{
    public int coinsForSuccessfulMatch = 300;
    public int coinsForGoodMatch = 200;
    public int coinsForTerribleMatch = 150;

    public GameObject successfulMatchGroup = null, goodMatchGroup = null, terribleMatchGroup = null;

    private int coinRewardAmount;
    private GameCanvasController gameCanvasController=null;
    public TMP_Text coinAmountRewardText = null;

    private void Awake()
    {
        // coinAmountRewardText.text=coinAmountReward+"";
    }

    private void Start()
    {
        
    }


    public void ApplyCoinReward()
    {
        //GameController.Instance.SetCoinAmount(GameController.CoinAmount+coinAmountReward);
        GameCanvasController.Instance.UpdateCoinAmountUI();
    }

    public void ActivateEOLScreenBasedOnMatchSuccessRate(float successRate)
    {
        if(gameCanvasController==null) gameCanvasController = GameCanvasController.Instance;
        
        if (successRate == 0f)  //kind of hard coded
        {
            coinRewardAmount = coinsForTerribleMatch;
            terribleMatchGroup.SetActive(true);
            if (gameCanvasController.terribleMatchAudioClip != null)
            {
                SoundManager.Instance.PlaySound(gameCanvasController.terribleMatchAudioClip, gameCanvasController.terribleMatchAudioClipVolume);
            }
        }
        else if (successRate == 0.5f)
        {
            coinRewardAmount = coinsForGoodMatch;
            goodMatchGroup.SetActive(true);
            if (gameCanvasController.goodMatchAudioClip != null)
            {
                SoundManager.Instance.PlaySound(gameCanvasController.goodMatchAudioClip, gameCanvasController.goodMatchAudioClipVolume);
            }
        }
        else if (successRate == 1f)
        {
            coinRewardAmount = coinsForSuccessfulMatch;
            successfulMatchGroup.SetActive(true);
            if (gameCanvasController.successfulMatchAudioClip != null)
            {
                SoundManager.Instance.PlaySound(gameCanvasController.successfulMatchAudioClip, gameCanvasController.successfulMatchAudioClipVolume);
            }
        }
        coinAmountRewardText.text = coinRewardAmount + "";
        GameController.CoinAmount += coinRewardAmount;

        gameObject.SetActive(true);
        gameCanvasController.coinUI.GetComponent<Animation>().Play("Coin UI Show");
    }
}
