using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "TargetSO", menuName = "SO/TargetSO")]
public class TargetSO : ScriptableObject
{
    public string ID;
    public string Name;
    public string Description;
    public int Type;
    public float Hp;
    public float Level;
    public float Speed;
    public float DamageRate;
}

[System.Serializable]
public class TargetData
{
    public string ID;
    public string Name;
    public string Description;
    public int Type;
    public float Hp;
    public float Level;
    public float Speed;
    public float DamageRate;
}
