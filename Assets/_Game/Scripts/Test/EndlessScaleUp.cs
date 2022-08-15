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
        contestantRB.AddForce((contestantRB.transform.up - contestantRB.transform.forward) * 15f, ForceMode.VelocityChange);
        transform.DOScaleY(3f, 0.25f);

        //asasdd
    }

    [ContextMenu("MoveUp")]
    public void MoveUp()
    {
        contestantRB.isKinematic = false;
        Vector3 a= contestantRB.transform.up - contestantRB.transform.forward;  //45 degree angle
        a=a.normalized;
        
       // Debug.Log(Vector3.Angle(a,-contestantRB.transform.forward));
      // Debug.DrawRay(contestantRB.transform.position, - contestantRB.transform.forward*10f, Color.red);
      // Debug.DrawRay(contestantRB.transform.position, a, Color.red);
      // Time.timeScale=0f;
        Vector3 thrirtyDegreeAngle=Vector3.RotateTowards(a,-contestantRB.transform.forward,-15f* Mathf.Deg2Rad,0.0f);

        Debug.Log(Vector3.Angle(thrirtyDegreeAngle,-contestantRB.transform.forward));
        contestantRB.AddForce((thrirtyDegreeAngle) * 23f, ForceMode.VelocityChange);
        transform.DOMoveY(transform.position.y + GetComponent<Renderer>().bounds.size.y, 0.3f);
        //asasdd
    }
    // Update is called once per frame
    void Update()
    {
        // transform.localScale=transform.localScale+new Vector3(0,5f * Time.deltaTime,0);
    }
}
