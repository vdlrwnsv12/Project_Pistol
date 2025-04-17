using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

[Serializable]
//public class PlayerGroundData
//{
//    [field: SerializeField]
//    [field: Range(0f, 25f)]
//    public float BaseSpeed { get; set; } = 1f;

//    [field: SerializeField]
//    [field: Range(0f, 25f)]


//    [field: Header("IdleData")]
//    [field: Header("WalkData")]

//    public float WalkSpeedModifier { get; private set; } = 0.5f;
//}

public class PlayerStateMachine : StateMachine
{
    public Player Player { get; }
    public Vector2 MovementInput { get; set; }
    public float MovementSpeed { get; private set; }
    public float RotationDamping { get; private set; }
    public float MovementSpeedModifier { get; set; } = 1f;

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
        MovementSpeed = player.statHandler.MovementSpeed;
    }
}
