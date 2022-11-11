using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remember : MonoBehaviour
{
    [SerializeField] private Chicken chicken;
    [SerializeField] private Animator animator;
    
    void EndAnimation()
    {
        animator.SetTrigger("idle");
        chicken.SetIdle();
    }
}
