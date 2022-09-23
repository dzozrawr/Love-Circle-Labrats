using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;
using System.Runtime.Serialization;

[DataContract]
public class SaveData
{
    [DataMember]
    public int level;
    [DataMember]
    public int coins;
    [DataMember]
    public int unchosenPlayerPrefabInstanceID;
    [DataMember]
    public int missionID;

    [DataMember]
    public List<SetShop.SetInfo> setsInShopInfos;




    public SaveData(int _level)
    {
        level = _level;
        missionID = GameController.missionID;
        coins = GameController.CoinAmount;   //implicit saving of the coin amount for simplicity of the constructor
        unchosenPlayerPrefabInstanceID = GameController.UnchosenPlayerPrefab.GetInstanceID();

    }

    private SaveData()
    {

    }


}
