using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Contestant;
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

    public AudioClip springAudioClip = null;
    [Range(0, 1f)]
    public float springAudioClipVolume = 1f;
    public AudioClip waterSplashAudioClip = null;
    [Range(0, 1f)]
    public float waterSplashAudioClipVolume = 0.5f;

    private GameObject chosenCurtain = null;

    private Bounds springBounds;

    protected override void Start()
    {
        base.Start();
        springBounds = eliminationSpringlist[0].GetComponent<Renderer>().bounds;
        RenderSettings.skybox = skybox;
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

        contestant.animator.SetTrigger("FreeFall");

        contestantCollider.enabled = true;
        contestantRB.centerOfMass = contestantCollider.center;
        Bounds contestantColliderBounds = contestantCollider.bounds;

        contestant.animator.enabled = false;
        contestant.SetRagdollRigidbodyState(true);
        contestant.SetColliderState(true);

        Rigidbody pelvisRb = contestant.GetPelvisRigidBody();

        pelvisRb.AddForceAtPosition(launchVector * eliminationForceMagnitude * 7f, contestant.transform.position + contestantRB.centerOfMass + new Vector3(Random.Range(-contestantColliderBounds.extents.x / 2, contestantCollider.bounds.extents.x / 2), Random.Range(-contestantCollider.bounds.extents.y / 2, contestantCollider.bounds.extents.y / 2), 0), ForceMode.VelocityChange);

        spring.transform.DOMoveY(spring.transform.position.y + springBounds.size.y, 0.3f);

        if (springAudioClip != null)
        {
            SoundManager.Instance.PlaySound(springAudioClip,springAudioClipVolume);
        }

        Invoke(nameof(PlayEliminationSoundAfterDelay), 0.5f);

    }

    private void PlayEliminationSoundAfterDelay()
    {
        if (eliminationAudioClip != null)
        {
            SoundManager.Instance.PlaySound(eliminationAudioClip, eliminationAudioClipVolume);
        }
    }

    public override void OpenPlayerCurtain(PlayerScript player)
    {
        if (player == GameController.Instance.playerL)
        {
            chosenCurtain = curtainL;
        }
        else
      if (player == GameController.Instance.playerR)
        {
            chosenCurtain = curtainR;
        }

        chosenCurtain.transform.DOScaleX(curtainCloseOffset, 0.75f).onComplete = () =>
        {
            GameController.Instance.CurtainOpen?.Invoke();
        };
    }
}
