using UnityEngine;

[CreateAssetMenu(menuName = "Achievement/New Achievement")]
public class AchievementData : ScriptableObject
{
    public string id;
    public string title;
    public string description;
    public AchievementConditionType conditionType;
    public float requiredValue;
}

public enum AchievementConditionType
{
    StageClearcount,
    ComboMax,
    HeadshotRatio,
    NoMissRun,
    HiddenAction,
    FastTargetKill,
}