using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerReloadState : PlayerBaseState
{
    public PlayerReloadState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entered Reload State");
       
        StartAnimation(stateMachine.Player.AnimationData.ReloadParamterHash);
    }
 
    public override void Update()
    {
        base.Update(); 

        AnimatorStateInfo stateInfo = stateMachine.Player.Animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Reload") && stateInfo.normalizedTime >= 1f) 
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }

    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.ReloadParamterHash);

    }

   

}
