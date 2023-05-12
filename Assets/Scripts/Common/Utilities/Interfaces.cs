using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Activate();
    void Deactivate();
    bool IsActive();
}

// This is for the Command Pattern
public interface ICommand
{ 
    void Execute();
    void Undo();
}
