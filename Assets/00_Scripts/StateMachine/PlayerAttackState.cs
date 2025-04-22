using UnityEngine;

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

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
    }

    public override void Update()
    {
        base.Update();
        var animInfo = stateMachine.Player.Animator.GetCurrentAnimatorStateInfo(0);

        // 애니메이션이 일정 진행률에 도달하면 한 번만 총알 발사
        if (!hasShot && animInfo.IsName("Attack") && animInfo.normalizedTime > 0)
        {
            hasShot = true;
            Shoot();
        }
        
        // 애니메이션 종료 시 Idle 상태로 전환
        if (animInfo.IsName("Attack") && animInfo.normalizedTime >= 1)
        {
            
            if (stateMachine.MovementInput != Vector2.zero)
            {
                stateMachine.ChangeState(stateMachine.WalkState);
                Debug.Log("walk 전환");
            }
            else
            {
                stateMachine.ChangeState(stateMachine.IdleState);
                Debug.Log("idle전환");
            }
        }
    }

    private void Shoot()
    {
        Transform cam = stateMachine.MainCamTransform;
        Ray ray = new Ray(cam.position, cam.forward);
        Debug.Log("슈팅");
        if (stateMachine.Player.Weapon.Controller != null)
        {
            Debug.Log("if문 슈팅");
            stateMachine.Player.Weapon.Controller.Fire(stateMachine.IsAds);
        }
    }
}
