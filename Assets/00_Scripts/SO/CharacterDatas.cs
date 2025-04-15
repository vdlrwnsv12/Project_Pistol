using UnityEngine;
using DataDeclaration;

[CreateAssetMenu(fileName = "Character", menuName = "SO/CharacterDatas")]

public class CharacterDatas : ScriptableObject
{
public string ID;
public string Name;
public string Description;
public float RCL;
public float HDL;
public float STP;
public float SPD;
public int Cost;
[field: SerializeField] public CharacterStat Stat { get; private set; }
[field: SerializeField] public PlayerGroundData GroundData { get; private set; }
}
