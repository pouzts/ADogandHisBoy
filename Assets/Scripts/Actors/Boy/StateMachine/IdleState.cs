using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class IdleState : State
{   
    public IdleState(string name, Agent agent) : base(name, agent)
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
        if (Agent.PlayerInSite)
        {
            Agent.transform.LookAt(Agent.player.gameObject.transform);
        }
    }
}
