using System.Collections.Generic;

#region Data Classes

/// <summary>
/// 룸 데이터 리스트 (JSON 루트)
/// </summary>
[System.Serializable]
public class RoomDataList
{
    public List<RoomData> Data;
}

/// <summary>
/// 단일 룸 데이터 구조
/// </summary>
[System.Serializable]
public class RoomData
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
#endregion