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
        
    }

    public override void OnUpdate()
    {
        if (Vector3.Distance(Agent.transform.position, Agent.Interactable.transform.position) < 0.2f && !Agent.NavMeshAgent.isStopped) 
        { 
            Agent.NavMeshAgent.isStopped = true;
        }

        if (!Agent.InteractInSite) 
        {

            return;
        }

        if (Agent.InteractInSite)
        {

            return;
        }

        Agent.NavMeshAgent.SetDestination(Agent.Interactable.transform.position);
    }
}
