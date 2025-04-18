using UnityEngine;

[CreateAssetMenu(fileName = "Stage", menuName = "SO/StageSO")]
public class StageSO : ScriptableObject
{
    public string ID;
    public string Name;
    public int Stage;
    public int Index;
    public string[] Targets;
    public string RoomID;
    public Vector3 RoomPos;
    public Vector3 RoomRot;
}
