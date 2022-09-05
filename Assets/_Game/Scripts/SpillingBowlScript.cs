using DG.Tweening;
using DG.Tweening.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpillingBowlScript : MonoBehaviour
{
    public float durationOfAnimation = 2f;
    public float delayBeforeAnimationStart = 0.4f;
    public float delayBeforeAnimationEnd = 0.5f;
    public GameObject bowlModelToDestroy = null;
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(Spill), delayBeforeAnimationStart);
        UnparentBowlContent();
    }

    [ContextMenu("Spill")]
    private void Spill()
    {
      //  transform.DOMove(new Vector3(0, transform.position.y, transform.position.z), durationOfAnimation);
        var tween= transform.DORotate(new Vector3(0, 0, 160), durationOfAnimation);
        tween.OnComplete(() =>
        {
            Invoke(nameof(DestroyBowl), delayBeforeAnimationEnd);
        }
        );
    }

    private void DestroyBowl() {
        if (bowlModelToDestroy)
        {
            Destroy(bowlModelToDestroy);
        }
        else
        {
            if (transform.childCount != 0)
            {
                Debug.LogError(transform.childCount);
                for (int i = 1; i < transform.childCount; i++)
                {
                    transform.GetChild(i).SetParent(transform.parent, true);
                }
            }
            Destroy(gameObject);
        }

    }
    private void UnparentBowlContent(){
        foreach (GameObject go in transform.GetComponentsInChildren<GameObject>())
        {
            if(go==bowlModelToDestroy) continue;
            go.transform.SetParent(transform.parent);
        }        
    }
}
