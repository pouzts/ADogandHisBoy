using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour, IInteractable
{
    private bool activated = false;

    public void Activate()
    {
        activated = true;
    }

    public void Deactivate()
    {
        activated = false;
    }

    public bool IsActive()
    {
        return activated;
    }
}
