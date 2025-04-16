using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "StageSO", menuName = "SO/StageSO")]
public class StageSO : ScriptableObject
{
    public string ID;
    public string Name;
    public string[] Targets;
}

[System.Serializable]
public class StageData
{
    public string ID;
    public string Name;
    public string[] Targets;
}
