using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanTrigger : MonoBehaviour
{
    public ParticleSystem splashParticle=null;
    private ContestantQuestioningManager contestantQuestioningManager;
    private void Start()
    {
        contestantQuestioningManager = ContestantQuestioningManager.Instance;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Contestant"))
        {
            //StartCoroutine(Splash(other.gameObject.transform.position));//add splash particle and whatnot, sound maybe
            Instantiate(splashParticle,other.gameObject.transform.position,Quaternion.identity) ;
            contestantQuestioningManager.ContestantEliminatedSignal();
            ContestantScript contestantScript= other.transform.GetComponentInParent<ContestantScript>();
            Destroy(contestantScript.gameObject,1f);
        }
    }

    IEnumerator Splash(Vector3 pos){
        Instantiate(splashParticle,pos,Quaternion.identity) ;
        yield return null;
    }
}
