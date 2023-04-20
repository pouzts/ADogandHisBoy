using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public interface IInteractable 
{
    void Activate();
    void Deactivate();
    bool IsActive();
}
