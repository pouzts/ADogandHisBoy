using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Activator
{
    [SerializeField] private bool canDeactivate = false;

    public override bool IsActivated 
    { 
        get => base.IsActivated;
        protected set 
        { 
            if (base.IsActivated && !canDeactivate)
                base.IsActivated = true;
            else
                base.IsActivated = value;
        } 
    }

    public override void OnActivate()
    {
        IsActivated = true;
        StartCoroutine(WaitForAnimation());
    }

    public override void OnDeactivate()
    {
        if (!canDeactivate) 
            return;

        IsActivated = false;
        StartCoroutine(WaitForAnimation());
    }

    private IEnumerator WaitForAnimation() 
    {
        if (TryGetComponent(out Animator anim))
        {
            anim.SetBool("IsActived", IsActivated);
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        }
    }
}
