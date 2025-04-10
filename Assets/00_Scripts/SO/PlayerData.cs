using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerGroundData
{
    [field:SerializeField][field: Range(0f, 25f)] public float BaseSpeed { get; set; } = 5f;
    [field:SerializeField][field: Range(0f, 25f)] public float BaseRotationDamping { get; private set; } = 1f;

    [field: Header("IdleData")]

    [field: Header("WalkData")]
    [field:SerializeField][field: Range(0f, 25f)] public float WalkSpeedModifier { get; private set; } = 0.5f;


}

[Serializable]
public class PlayerAttackData
{
    [field: SerializeField] public List<AttackInfoData> AttackInfoDatas { get; private set; }
    public int GetAttackInfoCount() { return AttackInfoDatas.Count; }  // To Do: 해보고 필요없으면 지우기

    public AttackInfoData GetAttackInfoData(int index) { return AttackInfoDatas[index]; }

}
[Serializable]
public class AttackInfoData
{
    [field: SerializeField] public string AttackName { get; private set; }
    [field: SerializeField] public int ComboStateIndex { get; private set; }    // 콤보 
    [field: SerializeField][field: Range(0f, 1f)] public float ComboTransitionTime { get; private set; }  // 콤보 시간 범위

    [field: SerializeField][field: Range(0f, 3f)] public float ForceTransitionTime { get; private set; }  // 얼마만큼 힘을 줄건지? To DO: 해보고 필요없으면 지울듯

    [field: SerializeField][field: Range(-10f, 10f)] public float Force { get; private set; } // 힘

    [field: SerializeField] public int Damage;

    [field: SerializeField] public float Dealing_Start_TransitionTime { get; private set; }
    [field: SerializeField] public float Dealing_End_TransitionTime { get; private set; }


}
[CreateAssetMenu(fileName = "Player", menuName = "SO/PlayerData")]
public class PlayerData : ScriptableObject
{
    [field: SerializeField] public PlayerGroundData GroundData { get; private set; }
    [field: SerializeField] public PlayerAttackData AttackData { get; private set; }
}
