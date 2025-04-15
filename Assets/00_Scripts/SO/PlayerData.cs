using System;
using DataDeclaration;
using UnityEngine;

[Serializable]
public class PlayerGroundData
{
    [field: SerializeField]
    [field: Range(0f, 25f)]
    public float BaseSpeed { get; set; } = 5f;

    [field: SerializeField]
    [field: Range(0f, 25f)]
  

    [field: Header("IdleData")]
    [field: Header("WalkData")]
   
    public float WalkSpeedModifier { get; private set; } = 0.5f;
}

//[CreateAssetMenu(fileName = "Player", menuName = "SO/PlayerData")]
//public class PlayerData : ScriptableObject
//{
//    [field: SerializeField] public string ID { get; private set; }
//    [field: SerializeField] public string Name { get; private set; }
//    [field: SerializeField] public PlayerGroundData GroundData { get; private set; }
//    [field: SerializeField] public CharacterStat Stat { get; private set; }
 
//    [field: SerializeField] public string name; // 캐릭터이름
//    [field: SerializeField] public float rcl;
//    [field: SerializeField] public float hdl;
//    [field: SerializeField] public float stp;
//    [field: SerializeField] public float spd;
//    [field: SerializeField] public float finalRecoil; //weaponFireController에 넣어놔서 삭제해도됨
//    [field: SerializeField] public int cost; // 
//}
