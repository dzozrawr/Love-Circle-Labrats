using DG.Tweening;
using DG.Tweening.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpillingBowlScript : MonoBehaviour
{
    public float durationOfAnimation = 2f;
    public float delayBeforeAnimationStart = 0.4f;
    public float delayBeforeAnimationEnd = 0.5f;
    public GameObject bowlModelToDestroy = null;

    private UnityEvent bowlDestroyed;

    public UnityEvent BowlDestroyed { get => bowlDestroyed; }

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
        var tween= transform.DOLocalRotate(new Vector3(transform.localRotation.x, transform.localRotation.y, 160), durationOfAnimation);
        tween.OnComplete(() =>
        {
            Invoke(nameof(DestroyBowl), delayBeforeAnimationEnd);
        }
        );
    }

    private void DestroyBowl() {
        bowlDestroyed?.Invoke();
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
        GameObject go=null;
        for (int i = 0; i < transform.childCount; i++)
        {
            go = transform.GetChild(i).gameObject;
            if (go == bowlModelToDestroy) continue;
            go.transform.SetParent(transform.parent.parent);
        }

    }
}
