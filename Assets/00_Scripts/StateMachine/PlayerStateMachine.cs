using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public Player Player { get; }

    public Vector2 MovementInput { get; set; }
    public float MovementSpeed { get;  set; }
    public float RotationDamping { get; private set; }
    public float MovementSpeedModifier { get; set; }

    public bool IsAttacking { get; set; }   
    public int ComboIndex { get; set; }
    public Transform MainCamTransform { get; set; }
    
    public PlayerIdleState IdleState { get; private set; }
    public PlayerWalkState WalkState { get; private set; }

    public PlayerAttackState AttackState { get; private set; }    

    public PlayerReloadState ReloadState { get; private set; }  
    public PlayerStateMachine(Player player)
    {
        this.Player = player;
        MainCamTransform = Camera.main.transform;

        IdleState = new PlayerIdleState(this);
        WalkState = new PlayerWalkState(this);
        AttackState = new PlayerAttackState(this);
        ReloadState = new PlayerReloadState(this);
         MovementSpeed = player.Data.GroundData.BaseSpeed;
         
    }
}
