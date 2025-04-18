using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "StageSO", menuName = "SO/StageSO")]
public class StageSO : ScriptableObject
{
    public string ID;
    public string Name;
    public int Stage;
    public int Index;
    public string[] Targets;
    public string RoomID;
    public float[] RoomPos;
    public float[] RoomRot;
}

[System.Serializable]
public class StageData
{
    public string ID;
    public string Name;
    public int Stage;
    public int Index;
    public string[] Targets;
    public string RoomID;
    public float[] RoomPos;
    public float[] RoomRot;
}
