using UnityEngine;

public class PlayerAdsState : PlayerBaseState
{
    private Transform handPos;
    private float playerHDL;
    
    public PlayerAdsState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        handPos = base.stateMachine.Player.HandPos.transform;
    }
    
    public override void Enter()
    {
        base.Enter();
        playerHDL = stateMachine.Player.Stat.HDL;
    }

    public override void Update()
    {
        base.Update();
    }
    
    private void WeaponShake()
    {
        var accuracy = Mathf.Clamp01((99f - playerHDL) / 98f);
        var shakeAmount = accuracy * 7.5f;

        var rotX = (Mathf.PerlinNoise(Time.time * 0.7f, 0f) - 0.5f) * shakeAmount;
        var rotY = (Mathf.PerlinNoise(0f, Time.time * 0.7f) - 0.5f) * shakeAmount * 3f;
        var rotZ = (Mathf.PerlinNoise(Time.time * 0.7f, Time.time * 0.7f) - 0.5f) * shakeAmount;

        var shakeRotation = Quaternion.Euler(rotX, rotY, rotZ);
        handPos.localRotation *= shakeRotation;
    }
}
