using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UIElements;

public class FollowState : State
{
    public FollowState(string name, Agent agent) : base(name, agent)
    {
    }

    public override void OnEnter()
    {
        Agent.NavMeshAgent.isStopped = false;
    }

    public override void OnExit()
    {
        Agent.NavMeshAgent.isStopped = true;
    }

    public override void OnUpdate()
    {
        Agent.NavMeshAgent.destination = Agent.player.gameObject.transform.position;
    }
}
