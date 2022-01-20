using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPersecutorState
{
    void ActualizeState();

    void AWanderState();

    void APersecutionState();

    void AWalkAwayState();
}
