using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackState : PlayerBaseState
{
    private bool hasShot; // 총 발사 여부

    public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        hasShot = false;
        
        StartAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
    }

    public override void Update()
    {
        base.Update();
        var animInfo = stateMachine.Player.Animator.GetCurrentAnimatorStateInfo(0);

        // 애니메이션이 일정 진행률에 도달하면 한 번만 총알 발사
        if (!hasShot && animInfo.IsName("Attack") && animInfo.normalizedTime >= 0.1f)
        {
            hasShot = true;
            Shoot();
        }

        // 애니메이션 종료 시 Idle 상태로 전환
        if (animInfo.IsName("Attack") && animInfo.normalizedTime >= 1f)
        {
            if (stateMachine.MovementInput != Vector2.zero)
                stateMachine.ChangeState(stateMachine.WalkState); 
            else
                stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
    }

   
    protected override void OnAttack(InputAction.CallbackContext context)
    {
        //// Idle에서 처리
        //if (stateMachine.MovementInput != Vector2.zero)
        //{
        //    stateMachine.ChangeState(stateMachine.WalkState);
        //    return;
        //}
        //stateMachine.ChangeState(stateMachine.IdleState);
    }

    private void Shoot()
    {
        
        Transform cam = stateMachine.MainCamTransform;
        Ray ray = new Ray(cam.position, cam.forward);
        Debug.Log("슈팅");
        if (stateMachine.Player.PlayerEquipment.fireController != null && stateMachine.Player.PlayerEquipment.fireController.isLocked)
        {
            Debug.Log("if문 슈팅");
            stateMachine.Player.PlayerEquipment.fireController.FireWeapon();
        }
    }

}
