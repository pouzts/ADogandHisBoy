using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Activate();
    void Deactivate();
    bool IsActive();
}

//
public interface ICommand
{ 
    void Execute();
    void Undo();
}

//public interface I
