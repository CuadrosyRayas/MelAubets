using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateMachine : MonoBehaviour
{
    public NavMeshAgent agent;
    public NavMeshAgent target;
    public FieldOfView fow;

    [Range(0, 10)] public float speed;
    [Range(1, 50)] public float walkRadius;

    [HideInInspector] public IPersecutorState currentState;
    [HideInInspector] public WanderState wanderState;
    [HideInInspector] public PersecutionState persecutionState;
    [HideInInspector] public WalkAwayState walkAwayState;

    private void Awake()
    {
        wanderState = new WanderState(this);
        persecutionState = new PersecutionState(this);
        walkAwayState = new WalkAwayState(this);

        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        agent.speed = speed;
        currentState = wanderState;
    }

    void Update()
    {
        currentState.ActualizeState();
    }
}
