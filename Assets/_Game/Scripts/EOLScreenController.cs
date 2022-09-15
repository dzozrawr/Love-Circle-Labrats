using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EOLScreenController : MonoBehaviour
{
    public int coinAmountReward=270;
    public TMP_Text coinAmountRewardText=null;

    private void Awake() {
       // coinAmountRewardText.text=coinAmountReward+"";
    }

    public void ApplyCoinReward(){
        GameController.Instance.SetCoinAmount(coinAmountReward);
    }
}
