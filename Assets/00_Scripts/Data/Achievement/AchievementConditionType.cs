using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AchievementConditionType
{
    /// <summary> 누적 킬 수 </summary>
    KillCount,

    /// <summary> 헤드샷 명중률 (퍼센트) </summary>
    HeadshotRatio,

    /// <summary> 특정 거리 이상에서 명중 </summary>
    DistanceShot,

    /// <summary> 연속 명중 횟수 </summary>
    ConsecutiveHits,

    /// <summary> 한 번도 빗나가지 않고 클리어 </summary>
    NoMissRun,

    /// <summary> 제한 시간 내 클리어 </summary>
    ClearTime,

    /// <summary> 연속 콤보 수 </summary>
    ComboCount,

    /// <summary> 특정 스테이지 클리어 </summary>
    StageClear,

    /// <summary> 특정 무기로 킬 </summary>
    WeaponSpecificKill
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
