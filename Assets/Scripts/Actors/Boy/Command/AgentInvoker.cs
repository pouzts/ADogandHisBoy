using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentInvoker
{
    private ICommand command;

    public AgentInvoker(ICommand initCommand)
    { 
        command = initCommand;
    }

    public void SetCommand(ICommand command)
    { 
        this.command = command;
    }

    public void ExecuteCommand()
    { 
        command.Execute();
    }
}
