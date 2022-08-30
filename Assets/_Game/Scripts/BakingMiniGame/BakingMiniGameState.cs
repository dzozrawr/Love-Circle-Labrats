using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BakingMiniGameState
{
    void InitState(BakingMiniGame bmg);
    BakingMiniGameState DoState(BakingMiniGame bmg);
}
