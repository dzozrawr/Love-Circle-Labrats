using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StudioSet : MonoBehaviour
{
    public PlayerScript playerL = null, playerR = null;

    protected ContestantQuestioningManager contestantQuestioningManager = null;
    private void Start()
    {
        contestantQuestioningManager=ContestantQuestioningManager.Instance;
    }
    public abstract void OpenPlayerCurtain(PlayerScript player);
    public abstract void EliminateContestant(ContestantScript contestant);
}
