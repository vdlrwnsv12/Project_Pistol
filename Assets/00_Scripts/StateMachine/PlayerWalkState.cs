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
        // 이 이벤트는 오직 한 번만 처리되도록 할 수 있음.
        // 예를 들어, 공격 입력이 "started" 이벤트라면,
        // 버튼이 눌리는 순간 전환하도록 합니다.
        stateMachine.ChangeState(stateMachine.AttackState);
    }
}
