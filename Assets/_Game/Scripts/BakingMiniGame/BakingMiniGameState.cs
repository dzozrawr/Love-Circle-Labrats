using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BakingMiniGameState
{
    BakingMiniGameState DoState(BakingMiniGame bmg);
}
