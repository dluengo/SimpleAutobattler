using System;
using UnityEngine;

public class ActionEndSMB : StateMachineBehaviour
{
    public event Action OnActionAnimEnd;

    // Called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnActionAnimEnd?.Invoke();
    }
}
