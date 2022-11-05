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


    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && chicken.getAction == CHICKEN_ACTION.IDLE && !chicken.isThink && chicken._targetDistance < 1.5f)
        {
            chicken.isThink = true;
            chicken.Think();
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            chicken.Setidle();
        }
    }
}
