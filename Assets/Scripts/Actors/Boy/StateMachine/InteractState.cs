using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractState : State
{
    public InteractState(string name, Agent agent) : base(name, agent)
    {
    }

    public override void OnEnter()
    {
        Agent.Interactable.GetComponent<IInteractable>().Activate();
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        
    }
}
