using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "RoomSO", menuName = "SO/RoomSO")]
public class RoomSO : ScriptableObject
{
    public string ID;
    public string[] Targets;
}

[System.Serializable]
public class RoomData
{
    public string ID;
    public string[] Targets;
}
