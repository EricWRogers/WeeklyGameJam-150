using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorStateHelper : MonoBehaviour
{
    public event AnimatorStateAction onStateEnter;
    public event AnimatorStateAction onStateUpdate;
    public event AnimatorStateAction onStateExit;
    public event AnimatorStateAction onStateMove;
    public event AnimatorStateAction onStateIK;

    private Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public void OnStateEnter(AnimatorStateHandler animatorStateHandler, AnimatorStateInfo stateInfo, int layerIndex)
    {
        onStateEnter?.Invoke(animatorStateHandler.stateIdentifier, _animator, stateInfo, layerIndex);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public void OnStateUpdate(AnimatorStateHandler animatorStateHandler, AnimatorStateInfo stateInfo, int layerIndex)
    {
        onStateUpdate?.Invoke(animatorStateHandler.stateIdentifier, _animator, stateInfo, layerIndex);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public void OnStateExit(AnimatorStateHandler animatorStateHandler, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log(animatorStateHandler.stateIdentifier + " Exit 2");
        onStateExit?.Invoke(animatorStateHandler.stateIdentifier, _animator, stateInfo, layerIndex);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    public void OnStateMove(AnimatorStateHandler animatorStateHandler, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that processes and affects root motion
        onStateMove?.Invoke(animatorStateHandler.stateIdentifier, _animator, stateInfo, layerIndex);
    }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    public void OnStateIK(AnimatorStateHandler animatorStateHandler, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that sets up animation IK (inverse kinematics)
        onStateIK?.Invoke(animatorStateHandler.stateIdentifier, _animator, stateInfo, layerIndex);
    }
}

public delegate void AnimatorStateAction(string stateIdentifier, Animator animator,
    AnimatorStateInfo stateInfo, int layerIndex);

public static class AnimatorHelperExtensions
{
    public static AnimatorStateHelper GetAnimatorHelper(this Animator self)
    {
        return self.GetComponent<AnimatorStateHelper>();
    }
}