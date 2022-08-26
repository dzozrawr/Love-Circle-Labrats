using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StudioSet : MonoBehaviour
{
    public PlayerScript playerL = null, playerR = null;

    public Material skybox;

    protected ContestantQuestioningManager contestantQuestioningManager = null;
    protected virtual void Start()
    {
        contestantQuestioningManager = ContestantQuestioningManager.Instance;
    }

    protected virtual void OnEnable()
    {
        RenderSettings.skybox = skybox;
    }
    public abstract void OpenPlayerCurtain(PlayerScript player);
    public abstract void EliminateContestant(ContestantScript contestant);
}
