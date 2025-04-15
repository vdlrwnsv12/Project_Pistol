using UnityEngine;
using System.Collections.Generic;
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
}
