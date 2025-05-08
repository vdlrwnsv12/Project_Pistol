using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms.Impl;

[CreateAssetMenu(fileName = "Achievement", menuName = "SO/Achievement")]
public class AchievementSO : ScriptableObject
{
    public string id;
    public string title;
    public string description;
    public AchievementConditionType conditionType;
    public float requiredValue;
}

public enum AchievementType
{
    KillCount,
    ComboCount,
    TimeSurvived,
    ItemCollected
}

public class AchievementListSO : ScriptableObject
{
    public List<AchievementSO> achievements;
}
