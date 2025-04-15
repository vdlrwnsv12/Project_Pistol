using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "Target", menuName = "SO/TargetDatas")]

public class TargetDatas : ScriptableObject
{
public string ID;
public string Name;
public string Description;
public int Type;
public float BaseHp;
public float Speed;
}
