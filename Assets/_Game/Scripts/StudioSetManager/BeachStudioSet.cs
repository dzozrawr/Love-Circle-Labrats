using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BeachStudioSet : StudioSet
{
    public GameObject curtainL = null, curtainR = null;

    public GameObject[] eliminationSpringlist = null;

    [Range(0, 100)]
    public float eliminationForceMagnitude = 23f;

    [Range(0, 44.99f)]
    public float launchVectorDegreeRandomizationRange = 0;

    [ContextMenuItem("Test curtain animation", "CurtainAnimation")]
    [Range(0, 1f)]
    public float curtainCloseOffset = 0f;

    private GameObject chosenCurtain = null;

    private Bounds springBounds;

    protected override void Start()
    {
        base.Start();
        springBounds = eliminationSpringlist[0].GetComponent<Renderer>().bounds;
    }

    private void CurtainAnimation()
    {
        curtainL.transform.DOScaleX(curtainCloseOffset, 0.75f).onComplete = () =>
      {
          GameController.Instance.CurtainOpen?.Invoke();
      };
    }

    public override void EliminateContestant(ContestantScript contestant)   //maybe this script should add rigid bodies to contestants
    {
        int contestantInd = contestantQuestioningManager.
        contestants.
        IndexOf(contestant);
        GameObject spring = eliminationSpringlist[contestantInd];

        Rigidbody contestantRB = contestant.GetComponent<Rigidbody>();
        BoxCollider contestantCollider = contestant.GetComponent<BoxCollider>();
        //contestantRB.isKinematic = false;
        Vector3 launchVector = contestantRB.transform.up - contestantRB.transform.forward;  //45 degree angle
        launchVector = launchVector.normalized;

        launchVector = Vector3.RotateTowards(launchVector, -contestantRB.transform.forward, Random.Range(-launchVectorDegreeRandomizationRange, launchVectorDegreeRandomizationRange) * Mathf.Deg2Rad, 0.0f);
        launchVector = launchVector.normalized;

        // Debug.Log(Vector3.Angle(launchVector,-contestantRB.transform.forward));
        contestant.animator.SetTrigger("FreeFall");

        contestantCollider.enabled = true;
        contestantRB.centerOfMass = contestantCollider.center;
        Bounds contestantColliderBounds = contestantCollider.bounds;
        // contestantRB.AddForceAtPosition(launchVector * eliminationForceMagnitude, contestant.transform.position + contestantRB.centerOfMass + new Vector3(Random.Range(-contestantColliderBounds.extents.x / 2, contestantCollider.bounds.extents.x / 2), Random.Range(-contestantCollider.bounds.extents.y / 2, contestantCollider.bounds.extents.y / 2), Random.Range(-contestantCollider.bounds.extents.z / 2, contestantCollider.bounds.extents.z / 2)), ForceMode.VelocityChange);

        contestant.animator.enabled = false;
        contestant.SetRagdollRigidbodyState(true);
        contestant.SetColliderState(true);

        Rigidbody pelvisRb = contestant.GetPelvisRigidBody();
        //contestantRB.AddForceAtPosition(launchVector * eliminationForceMagnitude, contestant.transform.position + contestantRB.centerOfMass +new Vector3(Random.Range(-contestantColliderBounds.extents.x/2,contestantCollider.bounds.extents.x/2),Random.Range(-contestantCollider.bounds.extents.y/2,contestantCollider.bounds.extents.y/2),0  ), ForceMode.VelocityChange);
        pelvisRb.AddForceAtPosition(launchVector * eliminationForceMagnitude*7f, contestant.transform.position + contestantRB.centerOfMass + new Vector3(Random.Range(-contestantColliderBounds.extents.x / 2, contestantCollider.bounds.extents.x / 2), Random.Range(-contestantCollider.bounds.extents.y / 2, contestantCollider.bounds.extents.y / 2), 0), ForceMode.VelocityChange);
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

        chosenCurtain.transform.DOScaleX(curtainCloseOffset, 0.75f).onComplete = () =>
        {
            GameController.Instance.CurtainOpen?.Invoke();
        };
    }
}
