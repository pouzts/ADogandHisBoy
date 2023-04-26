using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour, IInteractable
{
    private bool activated = false;

    public void Activate()
    {
        StartCoroutine(WaitForAnimation());
        activated = true;
    }

    public void Deactivate()
    {
        StartCoroutine(WaitForAnimation());
        activated = false;
    }

    public bool IsActive()
    {
        return activated;
    }

    IEnumerator WaitForAnimation() 
    { 
        if (TryGetComponent(out Animator anim)) 
        {
            anim.SetBool("IsActived", !activated);
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        }
    }
}
