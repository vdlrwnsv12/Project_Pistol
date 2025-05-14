using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AchievementConditionType
{
    [InspectorName("누적 킬 수")]
    KillCount,

    [InspectorName("헤드샷 명중률 (%)")]
    HeadshotRatio,

    [InspectorName("특정거리 이상에서 명중")]
    DistanceShot,

    [InspectorName("연속 명중 횟수")]
    ConsecutiveHits,

    [InspectorName("한번도 명중하지않고 클리어")]
    NoMissRun,

    [InspectorName("제한 시간내에 클리어")]
    ClearTime,

    [InspectorName("연속 콤보 수")]
    ComboCount,

    [InspectorName("특정 스테이지 클리어")]
    StageClear,

    [InspectorName("특정 무기로 킬")]
    WeaponSpecificKill
}

/// <summary>
/// 수치형 조건 비교 방식
/// </summary>
public enum ConditionOperator
{
    [InspectorName("같다 (==)")]
    Equal,

    [InspectorName("초과 (>)")]
    Greater,

    [InspectorName("이상 (>=)")]
    GreaterOrEqual,

    [InspectorName("미만 (<)")]
    Less,

    [InspectorName("이하 (<=)")]
    LessOrEqual
}

[System.Serializable]
public class AchievementCondition
{
    /// <summary> 체크할 조건의 타입 (예: 킬 수, 명중률 등) </summary>
    public AchievementConditionType conditionType;

    /// <summary> 비교 연산자 (초과/미만/이상/이하 등) </summary>
    public ConditionOperator comparison;

    /// <summary> 목표 수치 값 </summary>
    public float targetValue;
}

public static class AchievementConditionTypeExtensions
{
    /// <summary>
    /// 조건 유형을 한글로 반환
    /// </summary>
    public static string ToKorean(this AchievementConditionType type)
    {
        switch (type)
        {
            case AchievementConditionType.KillCount:
                return "누적 킬 수";
            case AchievementConditionType.HeadshotRatio:
                return "헤드샷 명중률";
            case AchievementConditionType.DistanceShot:
                return "장거리 명중";
            case AchievementConditionType.ConsecutiveHits:
                return "연속 명중";
            case AchievementConditionType.NoMissRun:
                return "노미스 클리어";
            case AchievementConditionType.ClearTime:
                return "제한 시간 클리어";
            case AchievementConditionType.ComboCount:
                return "연속 콤보";
            case AchievementConditionType.StageClear:
                return "스테이지 클리어";
            case AchievementConditionType.WeaponSpecificKill:
                return "특정 무기 킬";
            default:
                return "알 수 없음";
        }
    }
}
