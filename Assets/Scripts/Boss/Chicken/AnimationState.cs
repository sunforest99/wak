using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationState : StateMachineBehaviour
{
    [SerializeField] Chicken chicken;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (chicken == null)
        {
            chicken = animator.gameObject.GetComponent<Chicken>();
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        chicken.Setidle();
    }
}
