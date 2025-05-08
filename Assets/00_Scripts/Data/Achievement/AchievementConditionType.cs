using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AchievementConditionType
{
    StageClear,       // 스테이지 클리어 횟수
    ComboCount,       // 최대 콤보 수
    KillCount,        // 적 처치 수
    HeadshotRatio,    // 헤드샷 비율
    TimeSurvived,     // 생존 시간
    ItemCollected,    // 아이템 수집 수
    NoMissRun,        // 무피격 클리어
    HiddenAction,     // 숨겨진 행동
    FastTargetKill    // 빠른 표적 처치
}