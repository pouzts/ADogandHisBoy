using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindCommand : ICommand
{
    private readonly Agent agent;

    public FindCommand(Agent agent)
    {
        this.agent = agent;
    }

    public void Execute()
    {
        agent.FollowPlayer();
    }
}
