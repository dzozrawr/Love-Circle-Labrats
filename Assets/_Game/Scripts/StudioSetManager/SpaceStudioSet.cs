using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using System;

public class SpaceStudioSet : StudioSet
{
    public GameObject curtainL = null, curtainR = null;

    public GameObject[] alienTubes = null;

    public GameObject[] eliminatedContestantsSpacePos = null;

    public GameObject[] miniGameSpacePos = null;

    [Range(0f, 20f)]
    public float tubeOffsetFromGround = 0f;

    [Range(0f, 10f)]
    public float tubeDroppingDownDuration = 0.75f;

    [Range(0f, 10f)]
    public float eliminationDelayBeforeGoingUp = 0.5f;

    [Range(0f, 20f)]
    public float eliminationGoingUpTweenDuration = 1f;
    [Range(0f, 50f)]
    public float eliminationGoingUpEndHeight = 25f;

    [Range(0f, 30f)]
    public float contestantSpaceVelocity = 10f;

    [Range(0f, 1.8f)]
    public float contestantSpaceRotationIntensity = 0.45f;


    private GameObject chosenCurtain = null;
    private Bounds tubeBounds;

    private int spacePosInd = 0;

    private List<ContestantScript> eliminatedContestants = new List<ContestantScript>();



    protected override void Start()
    {
        base.Start();
        tubeBounds = alienTubes[0].GetComponent<Renderer>().bounds;
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

        chosenCurtain.transform.DOMoveY(chosenCurtain.transform.position.y + chosenCurtain.GetComponent<Renderer>().bounds.size.y, 0.75f).onComplete = () =>
        {
            GameController.Instance.CurtainOpen?.Invoke();
        };
    }

    public override void EliminateContestant(ContestantScript contestant)
    {
        contestant.animator.SetTrigger("Amaze"); //different animation

        eliminatedContestants.Add(contestant);

        int contestantInd = contestantQuestioningManager.contestants.IndexOf(contestant);
        GameObject tube = alienTubes[contestantInd];
        GameObject spacePos = eliminatedContestantsSpacePos[spacePosInd++];

        GameObject alienLight = tube.transform.GetChild(0).gameObject;
        tube.transform.DOMoveY(contestant.transform.position.y + tubeBounds.size.y + tubeOffsetFromGround, tubeDroppingDownDuration).onComplete = () =>
        {
            alienLight.transform.position = contestant.transform.position;
            alienLight.transform.position += new Vector3(0, alienLight.GetComponent<Renderer>().bounds.extents.y, 0);
            alienLight.SetActive(true);

            StartCoroutine(GoUpTweenAnimationAfterDelay(contestant, tube, spacePos, eliminationDelayBeforeGoingUp));
        };


        /*         hole.transform.DOMove(hole.transform.position + hole.transform.up * hole.GetComponent<Renderer>().bounds.size.x, 0.5f).onComplete = () =>
                {

                }; */
    }

    private IEnumerator GoUpTweenAnimationAfterDelay(ContestantScript c, GameObject tube, GameObject spacePos, float delay)
    {
        yield return new WaitForSeconds(delay);
        c.animator.SetTrigger("FreeFall");
        c.transform.DOMoveY(eliminationGoingUpEndHeight, eliminationGoingUpTweenDuration).onComplete = () =>
        {
            tube.transform.GetChild(0).gameObject.SetActive(false);
            tube.transform.DOMoveY(eliminationGoingUpEndHeight, 0.1f).onComplete = () =>
            {
                StartCoroutine(LaunchToSpace(c, spacePos));
            };



            //contestantQuestioningManager.ContestantEliminatedSignal();
        };
    }

    private IEnumerator LaunchToSpace(ContestantScript c, GameObject spacePos)
    {
        c.transform.position = spacePos.transform.position;
        yield return new WaitForFixedUpdate();

        Rigidbody rb = c.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = false;

        BoxCollider collider = c.GetComponent<BoxCollider>();
        collider.enabled = true;
        rb.centerOfMass = collider.center;
        Bounds colliderBounds = collider.bounds;
        float yOffset = UnityEngine.Random.Range(-contestantSpaceRotationIntensity, contestantSpaceRotationIntensity);

        rb.AddForceAtPosition(spacePos.transform.right * contestantSpaceVelocity, c.transform.position + rb.centerOfMass + new Vector3(0, yOffset, 0), ForceMode.VelocityChange);

    }

    [ContextMenu("ShowMiniGameSpaceContestants")]
    public void ShowMiniGameSpaceContestants()
    {
        int i = 0;
        Rigidbody rb;
        Vector3 oldAngularVelocity = Vector3.zero; ;
        foreach (ContestantScript c in eliminatedContestants)
        {
            rb = c.GetComponent<Rigidbody>();

            oldAngularVelocity = rb.angularVelocity;
            rb.isKinematic = true;
            rb.isKinematic = false;
            /*             rb.velocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero; */

            c.transform.position = miniGameSpacePos[i].transform.position;
            c.transform.forward = -miniGameSpacePos[i].transform.forward;

            Vector3 r = transform.right * 2f;

            //rb.AddForceAtPosition(miniGameSpacePos[i].transform.right * contestantSpaceVelocity, c.transform.position + rb.centerOfMass + new Vector3(0, UnityEngine.Random.Range(-contestantSpaceRotationIntensity, contestantSpaceRotationIntensity), 0), ForceMode.VelocityChange);
            rb.AddForce(miniGameSpacePos[i].transform.right * contestantSpaceVelocity, ForceMode.VelocityChange);
            rb.AddTorque(transform.right * oldAngularVelocity.magnitude , ForceMode.VelocityChange);
            i++;
        }
    }

}
