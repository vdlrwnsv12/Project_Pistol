using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "StageSO", menuName = "SO/StageSO")]
public class StageSO : ScriptableObject
{
    public string ID;
    public int StageIndex;
    public string BaseRoom;
    public string[] Targets;
    public int[] RespawnPoints;
    public bool[] WallPoints;
    public int CivilianRespawn;
    public string Rail;
}

[System.Serializable]
public class StageData
{
    public string ID;
    public int StageIndex;
    public string BaseRoom;
    public string[] Targets;
    public int[] RespawnPoints;
    public bool[] WallPoints;
    public int CivilianRespawn;
    public string Rail;
}
