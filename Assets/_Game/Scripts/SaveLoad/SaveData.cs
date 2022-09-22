using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int level;
    public int coins;
    public int unchosenPlayerPrefabInstanceID;

    public int missionID;




    public SaveData(int _level)
    {
        level = _level;
        missionID=GameController.missionID;
        coins = GameController.CoinAmount;   //implicit saving of the coin amount for simplicity of the constructor
        unchosenPlayerPrefabInstanceID = GameController.UnchosenPlayerPrefab.GetInstanceID();
        
    }

    /*     public SaveData(int _level, int _coins, int _missionID, int _optionalLevelID)
        {
            level = _level;
            coins = _coins;
            missionID = _missionID;
            optionalLevelID = _optionalLevelID;
            unlockedProducts = ShopManager.GetUnlockedProducts();
            progressProducts = ShopManager.progressProducts;
            finishedProgressProducts = ShopManager.finishedProgressProducts;
            lastMissionStartedID = GameController.lastMissionStartedID;
            numberOfKeysCollected = GameController.numberOfKeysCollected;
        } */

    private SaveData()
    {

    }

    /*    public SaveData(int _level, int _coins, int _missionID)
        {
            level = _level;
            coins = _coins;
            missionID = _missionID;

        }*/
}
