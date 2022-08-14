using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EndlessScaleUp : MonoBehaviour
{
    public Rigidbody contestantRB = null;
    // Start is called before the first frame update
    void Start()
    {
        // transform.DOScaleY(10f, 5f);
    }
    [ContextMenu("ScaleUp")]
    public void ScaleUp()
    {
        contestantRB.isKinematic = false;
       // contestantRB.velocity=(contestantRB.transform.up-contestantRB.transform.forward)*15f;
        contestantRB.AddForce((contestantRB.transform.up-contestantRB.transform.forward)*15f,ForceMode.VelocityChange);
        transform.DOScaleY(3f, 0.25f);

        //asasdd
    }

    [ContextMenu("MoveUp")]
    public void MoveUp()
    {
        contestantRB.isKinematic = false;
        transform.DOMoveY(10f, 2f);
        //asasdd
    }
    // Update is called once per frame
    void Update()
    {
        // transform.localScale=transform.localScale+new Vector3(0,5f * Time.deltaTime,0);
    }
}
