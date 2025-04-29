using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "CharacterSO", menuName = "SO/CharacterSO")]
public class CharacterSO : ScriptableObject
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

[System.Serializable]
public class CharacterData
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
