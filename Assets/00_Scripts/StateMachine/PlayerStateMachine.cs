using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public Player Player { get; private set; }
    public Vector2 MovementInput;
    public Vector2 MouseInput;
    public float RotationX;

    public PlayerIdleState IdleState { get; private set; }
    public PlayerWalkState WalkState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerReloadState ReloadState { get; private set; }
    public PlayerAdsState AdsState { get; private set; }

    public bool IsAds;

    public PlayerStateMachine(Player player)
    {
        Player = player;

        IdleState = new PlayerIdleState(this);
        WalkState = new PlayerWalkState(this);
        AttackState = new PlayerAttackState(this);
        ReloadState = new PlayerReloadState(this);
        AdsState = new PlayerAdsState(this);

        IsAds = false;
    }
}
