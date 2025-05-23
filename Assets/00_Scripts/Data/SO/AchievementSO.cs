using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "AchievementSO", menuName = "SO/AchievementSO")]
public class AchievementSO : ScriptableObject
{
    public string ID;
    public string Name;
    public string Description;
    public float AchievementRate;
}

[System.Serializable]
public class AchievementData
{
    public string ID;
    public string Name;
    public string Description;
    public float AchievementRate;
}
[System.Serializable]
public class AchievementProgressData
{
    public string ID;
    public float CurrValue;

    public AchievementProgressData(string id, float value)
    {
        ID = id;
        CurrValue = value;
    }
}
