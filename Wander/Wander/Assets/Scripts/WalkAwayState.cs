using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAwayState : IPersecutorState
{
    private readonly StateMachine fsm;
    

    public WalkAwayState(StateMachine fsmPersecutor)
    {
        fsm = fsmPersecutor;
    }

    public void ActualizeState()
    {
        float distance = Vector3.Distance(fsm.agent.transform.position, fsm.target.transform.position);

        if (distance < fsm.persecutionState.maxDistance)
        {
            Vector3 dirToTarget = fsm.agent.transform.position - fsm.target.transform.position;
            Vector3 newPos = fsm.agent.transform.position + dirToTarget;
            fsm.agent.SetDestination(newPos);
        }
        else
        {
            fsm.persecutionState.maxDistance = 0;
            fsm.agent.SetDestination(fsm.persecutionState.lastPosition);
            if (fsm.fow.visibleTargets.Count != 0)
            {
                APersecutionState();
            }
            else if (fsm.agent.remainingDistance <= fsm.agent.stoppingDistance)
            {
                Debug.Log(fsm.persecutionState.lastPosition);
                AWanderState();
            }

        }
    }

    public void AWanderState()
    {
        Debug.Log("Wander...");
        fsm.currentState = fsm.wanderState;
    }

    public void APersecutionState()
    {
        Debug.Log("Persecution!");
        fsm.currentState = fsm.persecutionState;
    }

    public void AWalkAwayState()
    {
        Debug.Log("Can't change to the same state");
    }
}
