using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetShop : MonoBehaviour
{
    public enum SetID{
        None, Beach, Space
    }
    public class SetInfo{
        private SetID id;
        private bool isBought=false;

        public bool IsBought { get => isBought; set => isBought = value; }
        public SetID Id { get => id; set => id = value; }

        public SetInfo(SetID id)
        {
            this.id = id;
        }

    }

    public static List<SetInfo> setsInShopInfos= null;

    public List<SetPickingElement> setsInShopInstances=null;

    private void Awake() {
        if(setsInShopInfos==null){ //save file empty
            setsInShopInfos=new List<SetInfo>();
            foreach (SetPickingElement spe in setsInShopInstances)
            {
                if(spe.id==SetID.None) continue;
                setsInShopInfos.Add(new SetInfo(spe.id));
            }
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetSetAsBought(SetID id){
        foreach (SetInfo si in setsInShopInfos)
        {
            if(si.Id==id){
                si.IsBought=true;
            }
        }

        //save the game here (async)
    }


}
