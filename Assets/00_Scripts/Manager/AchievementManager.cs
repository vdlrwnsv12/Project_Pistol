using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 도전과제 조건 평가, 해금 처리, UI 연동을 담당하는 매니저
/// </summary>
public class AchievementManager : MonoBehaviour
{
    #region Fields

    /// <summary>전체 도전과제 리스트 (ScriptableObject)</summary>
    [SerializeField] private List<AchievementSO> allAchievements;

    /// <summary>달성된 도전과제 ID 목록</summary>
    private readonly HashSet<string> unlockedAchievements = new HashSet<string>();

    /// <summary>UI 팝업 및 리스트를 출력할 UI 뷰</summary>
    [SerializeField] private UIAchievementView uiView;

    /// <summary>글로벌 싱글톤 인스턴스</summary>
    public static AchievementManager Instance { get; private set; }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (allAchievements == null || allAchievements.Count == 0)
        {
            Debug.LogWarning("[AchievementManager] 도전과제가 등록되지 않았습니다.");
        }

        if (uiView == null)
        {
            Debug.LogWarning("[AchievementManager] UI View가 연결되지 않았습니다.");
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// 특정 조건 타입과 값에 따라 도전과제 달성 여부를 평가합니다.
    /// </summary>
    /// <param name="type">조건 타입</param>
    /// <param name="value">현재 값</param>
    public void CheckCondition(AchievementConditionType type, float value)
    {

        foreach (var achievement in allAchievements)
        {
            if (achievement == null) continue;

            if (achievement.conditionType == type &&
                value >= achievement.requiredValue &&
                !unlockedAchievements.Contains(achievement.id))
            {
                Unlock(achievement);
            }
        }
    }

    /// <summary>
    /// 달성한 도전과제를 반환합니다.
    /// </summary>
    /// <returns>달성된 도전과제 리스트</returns>
    public List<AchievementSO> GetUnlockedList()
    {
        List<AchievementSO> list = new List<AchievementSO>();

        foreach (var achievement in allAchievements)
        {
            if (achievement == null) continue;
            if (unlockedAchievements.Contains(achievement.id))
            {
                list.Add(achievement);
            }
        }

        return list;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// 도전과제를 해금하고 UI 팝업을 출력합니다.
    /// </summary>
    /// <param name="data">달성된 도전과제 데이터</param>
    private void Unlock(AchievementSO data)
    {
        unlockedAchievements.Add(data.id);
        Debug.Log($"[도전과제 달성] {data.title}");

        uiView?.ShowUnlockedPopup(data);
    }

    /// <summary>
    /// 해당 도전과제가 달성되었는지 확인합니다.
    /// </summary>
    public bool IsUnlocked(string id)
    {
        return unlockedAchievements.Contains(id);
    }

    #endregion
}
