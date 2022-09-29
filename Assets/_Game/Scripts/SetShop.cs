using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using UnityEngine.Events;

public class SetShop : MonoBehaviour
{
    private static SetShop instance=null;
    public static SetShop Instance { get => instance;  }
    public enum SetID{
        None, Beach, Space
    }

    [DataContract]
    public class SetInfo{

        [DataMember]
        private SetID id;
        [DataMember]
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

    [HideInInspector]
    public UnityEvent SetBought;

    

    private void Awake() {
        if(instance!=null){
            Destroy(gameObject);
            return;
        }
        instance=this;


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
                SetBought?.Invoke();
            }
        }

        //save the game here (async)
    }

    public static bool IsSetBought(SetID id){
        if(setsInShopInfos==null) return false;

        foreach (SetInfo si in setsInShopInfos)
        {
            if(si.Id==id){
                return si.IsBought;
            }
        }
        return false;
    }


}
