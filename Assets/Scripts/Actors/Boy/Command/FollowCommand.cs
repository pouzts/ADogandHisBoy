using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCommand : MonoBehaviour, ICommand
{
    private readonly Agent agent;

    private FollowCommand(Agent agent)
    {
        this.agent = agent;
    }

    public void Execute()
    {
        agent.FollowPlayer();
    }
}
