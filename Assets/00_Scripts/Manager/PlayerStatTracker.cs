using UnityEngine;

public class playerStatTracker : MonoBehaviour
{
    public int comboCount;
    public int headshotCount;
    public int totalShots;

    public void OnStageClear()
    {
        AchievementManager.Instance.CheckCondition
            (AchievementConditionType.StageClearcount, 1);
    }

    public void OnComboChanged(int combo)
    {
        AchievementManager.Instance.CheckCondition
            (AchievementConditionType.ComboMax, combo);
    }

    public void OnHeadshot()
    {
        headshotCount++;
    }

    public void OnShotFired()
    {
        totalShots++;
    }

    public void OnLevelEnd()
    {
        if (totalShots > 0)
        {
            float headshotRatio = (float)headshotCount / totalShots;
            AchievementManager.Instance.CheckCondition
                (AchievementConditionType.HeadshotRatio, headshotRatio);
        }
    }
}
