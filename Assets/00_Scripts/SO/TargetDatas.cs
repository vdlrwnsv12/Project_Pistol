using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "Target", menuName = "SO/TargetDatas")]

public class TargetDatas : ScriptableObject
{
public string ID;
public int Type;
public string TargetLevel;
public int HP;
public float DamageRate;
}
