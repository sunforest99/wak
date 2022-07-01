using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife_portal : DestroySelf
{
    [SerializeField] Animator animator;

    void Update()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 2.0f)
            destroySelf();
    }
}
