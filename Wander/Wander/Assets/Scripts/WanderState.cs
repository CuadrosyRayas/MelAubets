using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderState : IPersecutorState
{
    private readonly StateMachine fsm;

    public WanderState (StateMachine fsmPersecutor)
    {
        fsm = fsmPersecutor;
    }

    public Vector3 RandomNavMeshLocation()
    {
        Vector3 finalPosition = Vector3.zero;
        Vector3 randomPosition = Random.insideUnitSphere * fsm.walkRadius;
        randomPosition += fsm.transform.position;
        if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, fsm.walkRadius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    public void ActualizeState()
    {
        if (fsm.agent.remainingDistance <= fsm.agent.stoppingDistance)
        {
            fsm.agent.SetDestination(RandomNavMeshLocation());
        }

        if(fsm.fow.visibleTargets.Count != 0)
        {
            APersecutionState();
        }

    }

    public void AWanderState()
    {
        Debug.Log("Can't change to the same state");
    }

    public void APersecutionState()
    {
        Debug.Log("Persecution!");
        fsm.currentState = fsm.persecutionState;
    }

    public void AWalkAwayState()
    {
        Debug.Log("WALK AWAAAAAY!!");
        fsm.currentState = fsm.walkAwayState;
    }
}
