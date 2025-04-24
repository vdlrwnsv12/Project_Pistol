using UnityEngine;

public class PlayerReloadState : PlayerBaseState
{
    public PlayerReloadState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        if (!stateMachine.Player.Weapon.Controller.isReloading)
        {
            
            if (stateMachine.IsAds)
            {
                stateMachine.IsAds = false;
            }
            stateMachine.Player.Weapon.Controller.ReloadWeapon(stateMachine.IsAds);
        }
        StartAnimation(stateMachine.Player.AnimationData.ReloadParameterHash);
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
        StopAnimation(stateMachine.Player.AnimationData.ReloadParameterHash);
    }
}
