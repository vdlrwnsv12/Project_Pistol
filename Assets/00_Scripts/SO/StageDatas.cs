using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "Stage", menuName = "SO/StageDatas")]

public class StageDatas : ScriptableObject
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
