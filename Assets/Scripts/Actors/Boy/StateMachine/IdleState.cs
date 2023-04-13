using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class IdleState : State
{
    private readonly Agent agent;
    
    public IdleState(string name, Agent agent) : base(name)
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
        agent.transform.LookAt(agent.player.transform);
    }
}
