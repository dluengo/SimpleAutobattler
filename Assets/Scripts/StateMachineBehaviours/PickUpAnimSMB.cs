using System;
using UnityEngine;

public class PickUpAnimSMB : StateMachineBehaviour
{
    // --- Events ---
    public event Action OnPickUpAnimStart;
    public event Action OnPickUpAnimEnd;


    // --- State Machine Behaviour Callbacks ---
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnPickUpAnimStart?.Invoke();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnPickUpAnimEnd?.Invoke();
    }
}
