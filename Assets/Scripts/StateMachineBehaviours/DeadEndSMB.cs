using UnityEngine;
using System;

public class DeadEndSMB : StateMachineBehaviour
{
    public event Action OnDeadAnimEnd;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    var clips = animator.runtimeAnimatorController.animationClips;
    //    //foreach (var clip in clips)
    //    //{
    //    //    Debug.Log($"[DeadEndSMB] Animator has clip: {clip.name}, length: {clip.length}");
    //    //}
    //    //Debug.Log($"[DeadEndSMB] Entered Dead state at {Time.time}, state length: {stateInfo.length}, normalizedTime: {stateInfo.normalizedTime}");
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log($"Exited DeadEndSMB state at {Time.time}");
        OnDeadAnimEnd?.Invoke();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
