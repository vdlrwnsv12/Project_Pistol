using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "Stage", menuName = "SO/StageDatas")]

public class StageDatas : ScriptableObject
{
public string ID;
public string Name;
public string[] Targets;
}
