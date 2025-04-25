using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    // To Do: Exit 적으면 공격을 안하고 나가짐 확인할 것

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    public override void Update()
    {
        base.Update();
        if (stateMachine.MovementInput != Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.WalkState);
            return;
        }
    }
    
    protected override void OnAttack(InputAction.CallbackContext context)
    {
        // 버튼이 눌리는 순간 실행
        stateMachine.ChangeState(stateMachine.AttackState);
    }
    
    protected override void OnReload(InputAction.CallbackContext context)
    {
        base.OnReload(context);
        stateMachine.ChangeState(stateMachine.ReloadState);
    }
}
