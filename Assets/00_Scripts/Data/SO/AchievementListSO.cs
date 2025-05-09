using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms.Impl;

[CreateAssetMenu(fileName = "Achievement", menuName = "SO/Achievement")]
public class AchievementSO : ScriptableObject
{
    [Header("기본 정보")]
    public string id;
    public string title;
    [TextArea] public string description;

    [Header("조건")]
    public AchievementConditionType conditionType;  // 어떤 조건에 해당하는지
    public float requiredValue;                     // 달성 조건 수치
}

public enum AchievementType
{
    StageClearCount,   // 스테이지 클리어 횟수
    ComboMax,          // 최대 콤보 수
    HeadshotRatio,     // 헤드샷 비율
    NoMissRun,         // 무피격 또는 미스 없는 클리어
    HiddenAction,      // 숨겨진 행동 감지
    FastTargetKill     // 일정 시간 내 표적 처치
}

public class AchievementListSO : ScriptableObject
{
    public List<AchievementSO> achievements;
}
