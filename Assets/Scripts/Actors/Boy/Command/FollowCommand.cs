using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCommand : ICommand
{
    private readonly Agent agent;

    public FollowCommand(Agent agent)
    {
        this.agent = agent;
    }

    public void Execute()
    {
        agent.FollowPlayer();
    }
}
