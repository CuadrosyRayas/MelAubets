using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]

public class PersecutionState : IPersecutorState

{
    private readonly StateMachine fsm;
    public Vector3 lastPosition;
    public float maxDistance;

    public PersecutionState(StateMachine fsmPersecutor)
    {
        fsm = fsmPersecutor;
    }

    public void ActualizeState()
    {
        if (fsm.fow.visibleTargets.Count != 0)
        {
            lastPosition = fsm.fow.visibleTargets[0].position;
            fsm.agent.SetDestination(lastPosition);

            if (Vector3.Distance(fsm.agent.transform.position, fsm.target.transform.position) <= 3)
            {
                maxDistance = 10f;
                AWalkAwayState();
            }
        }
        else if (fsm.agent.remainingDistance <= fsm.agent.stoppingDistance)
        {
            AWanderState();
        }


    }

    public void AWanderState()
    {
        Debug.Log("Wander...");
        fsm.currentState = fsm.wanderState;
    }

    public void APersecutionState()
    {
        Debug.Log("Can't change to the same state");
    }

    public void AWalkAwayState()
    {
        Debug.Log("WALK AWAAAAAY!!");
        fsm.currentState = fsm.walkAwayState;
    }

}
