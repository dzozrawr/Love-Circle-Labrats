using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BeachStudioSet : StudioSet
{
    public GameObject curtainL = null, curtainR = null;

    public GameObject[] eliminationSpringlist = null;

    private GameObject chosenCurtain = null;

    private Bounds springBounds;

    protected override void Start()
    {
        base.Start();
        springBounds = eliminationSpringlist[0].GetComponent<Renderer>().bounds;
    }

    public override void EliminateContestant(ContestantScript contestant)   //maybe this script should add rigid bodies to contestants
    {


        int contestantInd = contestantQuestioningManager.
        contestants.
        IndexOf(contestant);
        GameObject spring = eliminationSpringlist[contestantInd];

        Rigidbody contestantRB = contestant.GetComponent<Rigidbody>();
        BoxCollider contestantCollider = contestant.GetComponent<BoxCollider>();
        contestantRB.isKinematic = false;
        Vector3 a = contestantRB.transform.up - contestantRB.transform.forward;  //45 degree angle
        a = a.normalized;

        // Debug.Log(Vector3.Angle(a,-contestantRB.transform.forward));
        // Debug.DrawRay(contestantRB.transform.position, - contestantRB.transform.forward*10f, Color.red);
        // Debug.DrawRay(contestantRB.transform.position, a, Color.red);
        // Time.timeScale=0f;
        // Vector3 thrirtyDegreeAngle=Vector3.RotateTowards(a,-contestantRB.transform.forward,-15f* Mathf.Deg2Rad,0.0f);

        // Debug.Log(Vector3.Angle(thrirtyDegreeAngle,-contestantRB.transform.forward));
        contestant.animator.SetTrigger("FreeFall");
        //contestantRB.AddForceAtPosition()

        contestantCollider.enabled = true;
        contestantRB.centerOfMass=contestantCollider.center;
        Bounds contestantColliderBounds=contestantCollider.bounds;
        contestantRB.AddForceAtPosition(a * 23f, contestant.transform.position + contestantRB.centerOfMass +new Vector3(Random.Range(-contestantColliderBounds.extents.x/2,contestantCollider.bounds.extents.x/2),Random.Range(-contestantCollider.bounds.extents.y/2,contestantCollider.bounds.extents.y/2),Random.Range(-contestantCollider.bounds.extents.z/2,contestantCollider.bounds.extents.z/2)  ), ForceMode.VelocityChange);
        //contestantRB.AddForce(a * 23f, ForceMode.VelocityChange);



        contestantCollider.isTrigger = true;


        spring.transform.DOMoveY(spring.transform.position.y + springBounds.size.y, 0.3f);
    }

    public override void OpenPlayerCurtain(PlayerScript player)
    {
        if (player == playerL)
        {
            chosenCurtain = curtainL;
        }
        else
      if (player == playerR)
        {
            chosenCurtain = curtainR;
        }

        chosenCurtain.transform.DOScale(Vector3.zero, 0.75f).onComplete = () =>
        {
            GameController.Instance.CurtainOpen?.Invoke();
        };
    }
}
