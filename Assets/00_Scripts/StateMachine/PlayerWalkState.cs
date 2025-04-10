using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkState : PlayerGroundState
{
    public PlayerWalkState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = groundData.WalkSpeedModifier;
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.WalkParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.WalkParameterHash);
    }
    protected override void OnAttack(InputAction.CallbackContext context)
    {
      
        stateMachine.ChangeState(stateMachine.AttackState);
    }
    protected override void OnReload(InputAction.CallbackContext context)
    {
        base.OnReload(context);
        Debug.Log("walk 리로드 하라고");
        stateMachine.ChangeState(stateMachine.ReloadState);
    }
}
