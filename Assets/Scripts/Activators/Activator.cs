using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Activator : MonoBehaviour
{
    [SerializeField] private List<IInteractable> interactables;
    public virtual bool IsActivated { get; protected set; } = false;

    private void Update()
    {
        IsActivated = IsAllActivated();

        if (IsActivated)
        {
            OnActivate();
        }
        else
        {
            OnDeactivate();
        }
    }

    public bool IsAllActivated()
    {
        foreach (var interactable in interactables)
        {
            if (!interactable.IsActive())
            {
                return false;
            }
        }

        return true;
    }

    public abstract void OnActivate();
    public abstract void OnDeactivate();
}
