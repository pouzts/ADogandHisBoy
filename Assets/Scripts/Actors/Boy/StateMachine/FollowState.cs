using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowState : State
{
    private Agent agent;

    public FollowState(string name, Agent agent) : base(name)
    {
        this.agent = agent;
    }

    public override void OnEnter()
    {
        
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {
        agent.transform.position = Vector3.MoveTowards(agent.transform.position, agent.player.transform.position, 0.1f);
    }
}
