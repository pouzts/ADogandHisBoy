using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable 
{
    void Activate();
    void Deactivate();
    bool IsActive();
}
