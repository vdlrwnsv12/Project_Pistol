using System;
using UnityEngine;

[Serializable]
public class PlayerGroundData
{
    [SerializeField][field: Range(0f, 25f)] public float BaseSpeed { get; set; } = 5f;
    [SerializeField][field: Range(0f, 25f)] public float BaseRotationDamping { get; private set; } = 1f;

    [field: Header("IdleData")]

    [field: Header("WalkData")]
    [SerializeField][field: Range(0f, 25f)] public float WalkSpeedModifier { get; private set; } = 0.5f;

   
}
[CreateAssetMenu(fileName = "Player", menuName = "SO/PlayerData")]
public class PlayerData : ScriptableObject
{
    [field:SerializeField] public PlayerGroundData GroundData { get; private set;}  

}
