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
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.WalkParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.WalkParameterHash);
    }
    protected override void OnAttack(InputAction.CallbackContext context) // 걷고 공격
    {
        Debug.Log("Walk에서 Attack!");
        base.OnAttack(context);
        stateMachine.ChangeState(stateMachine.AttackState);
        
    }
    protected override void OnReload(InputAction.CallbackContext context) // 걷고 리로드
    {
        base.OnReload(context);
        stateMachine.ChangeState(stateMachine.ReloadState);
    }
}
