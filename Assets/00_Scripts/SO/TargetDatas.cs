using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "Target", menuName = "SO/TargetDatas")]

public class TargetDatas : ScriptableObject
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
