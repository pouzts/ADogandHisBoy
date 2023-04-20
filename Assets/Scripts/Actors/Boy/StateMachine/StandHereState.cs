using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandHereState : State
{
    private Vector3 positionTo = Vector3.zero;

    public StandHereState(string name, Agent agent) : base(name, agent)
    {
    }

    public override void OnEnter()
    {
        positionTo = Agent.player.gameObject.transform.position;
        Agent.NavMeshAgent.isStopped = false;
    }

    public override void OnExit()
    {
        Agent.NavMeshAgent.isStopped = true;
    }

    public override void OnUpdate()
    {
        Agent.NavMeshAgent.destination = positionTo;

        if (Vector3.Distance(Agent.transform.position, positionTo) < 0.5f)
        { 
            Agent.StateMachine.SetState(Agent.StateMachine.GetState("IdleState"));
        }
    }
}
