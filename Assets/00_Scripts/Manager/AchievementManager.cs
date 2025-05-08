using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    [SerializeField] private List<AchievementSO> allAchievements;
    private HashSet<string> unlockedAchievements = new HashSet<string>();
    [SerializeField] private UIAchievementView uiView;

    public static AchievementManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void CheckCondition(AchievementConditionType type, float value)
    {
        Debug.Log($"[도전과제 체크] {type} = {value}");

        foreach (var achievement in allAchievements)
        {
            if (achievement.conditionType == type &&
                value >= achievement.requiredValue &&
                !unlockedAchievements.Contains(achievement.id))
            {
                Unlock(achievement);
            }
        }
    }

    private void Unlock(AchievementSO data)
    {
        Debug.Log($"[달성됨] {data.title}");
        unlockedAchievements.Add(data.id);
        uiView?.ShowUnlockedPopup(data);
    }

    public List<AchievementSO> GetUnlockedList()
    {
        var list = new List<AchievementSO>();
        foreach (var a in allAchievements)
        {
            if (unlockedAchievements.Contains(a.id))
                list.Add(a);
        }
        return list;
    }
}
