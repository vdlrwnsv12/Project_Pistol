using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 도전과제 조건 평가, 해금 처리, UI 연동을 담당하는 매니저
/// </summary>
public class AchievementManager : SingletonBehaviour<AchievementManager>
{
    #region Fields

    [Header("도전과제 설정")]

    /// <summary>전체 도전과제 리스트 (ScriptableObject)</summary>
    [SerializeField] private List<AchievementSO> allAchievements;

    [SerializeField] private PlayerStatTracker tracker;

    /// <summary>달성된 도전과제 ID 목록</summary>
    private readonly HashSet<string> unlockedAchievements = new();

    [Header("UI 연결")]

    /// <summary>도전과제 리스트 및 상세 정보 UI</summary>
    [SerializeField] private AchievementMainUI uiView;

    /// <summary>팝업 UI</summary>
    [SerializeField] private UIAchievementPopup popupUI;

    #endregion

    #region Unity Methods

    #endregion

    #region Achievement Check

    /// <summary>
    /// 도전과제 클리어 여부 전수 검사
    /// </summary>
    public void CheckAllAchievements()
    {
        foreach (var achievement in allAchievements)
        {
            if (IsUnlocked(achievement.id)) continue;

            if (IsConditionMet(achievement))
            {
                UnlockAchievement(achievement);
            }
        }
    }

    /// <summary>
    /// 도전과제 하나의 모든 조건이 충족되었는지 검사
    /// </summary>
    /// <param name="data">도전과제 SO</param>
    /// <returns>모든 조건 충족 여부</returns>
    private bool IsConditionMet(AchievementSO data)
    {
        foreach (var condition in data.conditions)
        {
            float value = GetStatValue(condition.conditionType);

            switch (condition.comparison)
            {
                case ConditionOperator.Equal:
                    if (value != condition.targetValue) return false;
                    break;

                case ConditionOperator.Greater:
                    if (value <= condition.targetValue) return false;
                    break;

                case ConditionOperator.GreaterOrEqual:
                    if (value < condition.targetValue) return false;
                    break;

                case ConditionOperator.Less:
                    if (value >= condition.targetValue) return false;
                    break;

                case ConditionOperator.LessOrEqual:
                    if (value > condition.targetValue) return false;
                    break;
            }
        }

        return true; // 모든 조건 통과 시 클리어
    }

    /// <summary>
    /// 도전과제를 달성 처리 (UI 연동 포함 예정)
    /// </summary>
    /// <param name="data">달성된 도전과제</param>
    private void UnlockAchievement(AchievementSO data)
    {
        unlockedAchievements.Add(data.id);
        Debug.Log($"[도전과제] '{data.title}' 달성됨!");
        popupUI?.ShowPopup(data);
        uiView?.ShowList(allAchievements); // 필요 시 UI 갱신
    }

    /// <summary>
    /// 특정 도전과제 ID가 달성되었는지 확인
    /// </summary>
    public bool IsUnlocked(string id)
    {
        return unlockedAchievements.Contains(id);
    }

    #endregion

    #region Helper

    /// <summary>
    /// 조건 유형에 따라 현재 수치를 반환 (PlayerStatTracker 참조)
    /// </summary>
    private float GetStatValue(AchievementConditionType type)
    {
        return type switch
        {
            AchievementConditionType.KillCount => tracker.killCount,
            AchievementConditionType.HeadshotRatio => tracker.HeadshotRatio,
            AchievementConditionType.DistanceShot => tracker.longestShotDistance,
            AchievementConditionType.ConsecutiveHits => tracker.consecutiveHitCount,
            AchievementConditionType.NoMissRun => tracker.noMissCount,
            AchievementConditionType.ClearTime => tracker.clearTimeSeconds,
            AchievementConditionType.ComboCount => tracker.comboCount,
            AchievementConditionType.StageClear => tracker.stageClearCount,
            AchievementConditionType.WeaponSpecificKill => tracker.weaponSpecificKillCount,
            _ => 0f
        };
    }

    #endregion
}
