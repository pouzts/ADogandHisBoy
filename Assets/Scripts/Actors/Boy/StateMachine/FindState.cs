using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FindState : State
{
    public FindState(string name, Agent agent) : base(name, agent)
    {
    }

    public override void OnEnter()
    {
        
    }

    public override void OnExit()
    {
        Agent.NavMeshAgent.isStopped = true;
    }

    public override void OnUpdate()
    {
        Agent.NavMeshAgent.SetDestination(Agent.Interactable.transform.position);
    }
}
