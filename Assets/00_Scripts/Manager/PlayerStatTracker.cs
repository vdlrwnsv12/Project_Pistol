using UnityEngine;

/// <summary>
/// 플레이어의 행동을 추적하여 도전과제 조건을 전달하는 클래스
/// </summary>
public class PlayerStatTracker : MonoBehaviour
{
    #region Fields

    private int comboCount;
    private int killCount;
    private int itemCollected;
    private int headshotCount;
    private int totalShots;
    private float survivalTime;
    private bool isNoMissRun = true;

    #endregion

    #region Unity Methods

    private void Update()
    {
        survivalTime += Time.deltaTime;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// 콤보 수가 변경되었을 때 호출합니다.
    /// </summary>
    /// <param name="combo">현재 콤보 수</param>
    public void OnComboChanged(int combo)
    {
        comboCount = combo;
        AchievementManager.Instance?.CheckCondition(AchievementConditionType.ComboCount, combo);
    }

    /// <summary>
    /// 적을 처치했을 때 호출합니다.
    /// </summary>
    public void OnEnemyKilled()
    {
        killCount++;
        AchievementManager.Instance?.CheckCondition(AchievementConditionType.KillCount, killCount);
    }

    /// <summary>
    /// 아이템을 획득했을 때 호출합니다.
    /// </summary>
    public void OnItemCollected()
    {
        itemCollected++;
        AchievementManager.Instance?.CheckCondition(AchievementConditionType.ItemCollected, itemCollected);
    }

    /// <summary>
    /// 헤드샷을 성공했을 때 호출합니다.
    /// </summary>
    public void OnHeadshot()
    {
        headshotCount++;
        totalShots++;
        float ratio = (float)headshotCount / totalShots;
        AchievementManager.Instance?.CheckCondition(AchievementConditionType.HeadshotRatio, ratio);
    }

    /// <summary>
    /// 총을 발사했을 때 호출합니다.
    /// </summary>
    public void OnShotFired()
    {
        totalShots++;
    }

    /// <summary>
    /// 피격되었을 때 호출합니다.
    /// </summary>
    public void OnMiss()
    {
        isNoMissRun = false;
    }

    /// <summary>
    /// 스테이지가 클리어되었을 때 호출합니다.
    /// </summary>
    public void OnStageClear()
    {
        AchievementManager.Instance?.CheckCondition(AchievementConditionType.StageClear, 1);
    }

    /// <summary>
    /// 레벨이 종료될 때 조건을 최종 평가합니다.
    /// </summary>
    public void OnLevelEnd()
    {
        AchievementManager.Instance?.CheckCondition(AchievementConditionType.StageClear, 1);

        if (isNoMissRun)
        {
            AchievementManager.Instance?.CheckCondition(AchievementConditionType.NoMissRun, 1);
        }

        AchievementManager.Instance?.CheckCondition(AchievementConditionType.TimeSurvived, survivalTime);

        ResetStats();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// 다음 레벨을 위해 내부 통계값을 초기화합니다.
    /// </summary>
    private void ResetStats()
    {
        comboCount = 0;
        killCount = 0;
        itemCollected = 0;
        headshotCount = 0;
        totalShots = 0;
        survivalTime = 0f;
        isNoMissRun = true;
    }

    #endregion
}