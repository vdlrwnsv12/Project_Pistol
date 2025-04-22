using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public Player Player { get; }
    public Vector2 MovementInput { get; set; }
    public float MovementSpeed { get; private set; }
    public float RotationDamping { get; private set; }
    public float MovementSpeedModifier { get; set; } = 1f;

    public bool IsAttacking { get; set; }
    public Transform MainCamTransform { get; set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerWalkState WalkState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerReloadState ReloadState { get; private set; }
    public PlayerAdsState AdsState { get; private set; }
    
    public bool IsAds { get; set; }

    public PlayerStateMachine(Player player)
    {
        Player = player;
        MainCamTransform = Camera.main.transform;

        IdleState = new PlayerIdleState(this);
        WalkState = new PlayerWalkState(this);
        AttackState = new PlayerAttackState(this);
        ReloadState = new PlayerReloadState(this);
        AdsState = new PlayerAdsState(this);

        IsAds = false;
    }
}
