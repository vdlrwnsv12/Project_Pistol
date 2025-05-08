using UnityEngine;

public class PlayerStatTracker : MonoBehaviour
{
    private int comboCount;
    private int killCount;
    private int itemCollected;
    private int headshotCount;
    private int totalShots;
    private float survivalTime;
    private bool isNoMissRun = true;

    private void Update()
    {
        survivalTime += Time.deltaTime;
    }

    public void OnComboChanged(int combo)
    {
        comboCount = combo;
        AchievementManager.Instance?.CheckCondition(AchievementConditionType.ComboCount, combo);
    }

    public void OnEnemyKilled()
    {
        killCount++;
        AchievementManager.Instance?.CheckCondition(AchievementConditionType.KillCount, killCount);
    }

    public void OnItemCollected()
    {
        itemCollected++;
        AchievementManager.Instance?.CheckCondition(AchievementConditionType.ItemCollected, itemCollected);
    }

    public void OnHeadshot()
    {
        headshotCount++;
        totalShots++;
        float ratio = (float)headshotCount / totalShots;
        AchievementManager.Instance?.CheckCondition(AchievementConditionType.HeadshotRatio, ratio);
    }

    public void OnShotFired()
    {
        totalShots++;
    }

    public void OnMiss()
    {
        isNoMissRun = false;
    }

    public void OnStageClear()
    {
        AchievementManager.Instance?.CheckCondition(AchievementConditionType.StageClear, 1);

        if (isNoMissRun)
        {
            AchievementManager.Instance?.CheckCondition(AchievementConditionType.NoMissRun, 1);
        }

        AchievementManager.Instance?.CheckCondition(AchievementConditionType.TimeSurvived, survivalTime);

        comboCount = 0;
        killCount = 0;
        itemCollected = 0;
        headshotCount = 0;
        totalShots = 0;
        survivalTime = 0f;
        isNoMissRun = true;
    }
}
