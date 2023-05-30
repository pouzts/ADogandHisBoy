using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandCommand : ICommand
{
    private readonly Agent agent;

    public StandCommand(Agent agent)
    { 
        this.agent = agent;
    }

    public void Execute()
    {
        agent.StandHere();
    }
}
