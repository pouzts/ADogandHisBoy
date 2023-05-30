using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandCommand : MonoBehaviour, ICommand
{
    private readonly Agent agent;

    private StandCommand(Agent agent)
    {
        this.agent = agent;
    }

    public void Execute()
    {
        agent.FollowPlayer();
    }
}
