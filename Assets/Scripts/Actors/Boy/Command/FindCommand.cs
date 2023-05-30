using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindCommand : MonoBehaviour
{
    private readonly Agent agent;

    private FindCommand(Agent agent)
    { 
        this.agent = agent;
    }

    public void Execute()
    {
        agent.FollowPlayer();
    }
}
