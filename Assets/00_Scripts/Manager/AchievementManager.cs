using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class AchievementManager : MonoBehaviour
{
    [SerializeField] private List<AchievementData> allAchievements;
    private HashSet<string> unlockedAchievements = new HashSet<string>();

    [SerializeField]

    public static AchievementManager Instance {  get; private set; }

    private void Awake()
    {
        if (Instance == null)
        { 
            Destroy(gameObject);
            return;
        }

        Instance = this;

    }

    public void CheckCondition(AchievementConditionType type, float value)
    {
        foreach (var allAchievement in allAchievements)
        {
            if (AchievementManager.conditionType == type &&
                value >= AchievementManager.requiredValue &&
                !unlockedAchievements.Contains(allAchievement.id))
            {
                unlock(allAchievement);
            }
        }
    }

    private void Unlock (AchievementData data)
    {
        unlockedAchievements.Add(data.id);
        uiView?.ShowUnlockedPopup(data);
    }

    public List<AchievementData> GetUnlockedList()
    {
        List<AchievementData> result = new List<AchievementData>();

        foreach (var achievement in allAchievements)
        {
            if (unlockedAchievements.Contains(achievement.id))
                result.Add(achievement);
        }
        return result;
    }
}