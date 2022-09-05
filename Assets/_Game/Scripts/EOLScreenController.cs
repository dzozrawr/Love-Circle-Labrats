using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EOLScreenController : MonoBehaviour
{
    public int coinAmountReward=270;
    public Text coinAmountRewardText=null;

    private void Awake() {
        coinAmountRewardText.text=coinAmountReward+"";
    }

    public void ApplyCoinReward(){
        GameController.Instance.SetCoinAmount(coinAmountReward);
    }
}
