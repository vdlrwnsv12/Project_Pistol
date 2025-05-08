using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 디버그용 버튼 UI를 통해 도전과제 조건을 강제로 트리거하는 테스트 클래스
/// </summary>
public class AchievementDebugUI : MonoBehaviour
{
    #region Fields

    [Header("버튼 연결")]
    [SerializeField] private Button headshotButton;
    [SerializeField] private Button comboButton;
    [SerializeField] private Button stageClearButton;
    [SerializeField] private Button showListButton;

    [Header("연동된 시스템")]
    [SerializeField] private PlayerStatTracker tracker;
    [SerializeField] private UIAchievementView uiView;

    #endregion

    #region Unity Methods

    private void Start()
    {
        if (tracker == null || uiView == null)
        {
            Debug.LogError("[DebugUI] tracker 또는 uiView 연결이 누락되었습니다.");
            return;
        }

        if (headshotButton != null)
        {
            headshotButton.onClick.AddListener(TriggerHeadshotTest);
        }

        if (comboButton != null)
        {
            comboButton.onClick.AddListener(TriggerComboTest);
        }

        if (stageClearButton != null)
        {
            stageClearButton.onClick.AddListener(TriggerStageClearTest);
        }

        if (showListButton != null)
        {
            showListButton.onClick.AddListener(TriggerListDisplay);
        }
    }

    #endregion

    #region Test Methods

    /// <summary>
    /// 헤드샷 테스트 (헤드샷 3회, 총격 4회 → 헤드샷 비율 계산)
    /// </summary>
    private void TriggerHeadshotTest()
    {
        Debug.Log("[DebugUI] 헤드샷 테스트 실행");
        tracker.OnHeadshot(); // 1
        tracker.OnHeadshot(); // 2
        tracker.OnShotFired(); // 일반 샷
        tracker.OnHeadshot(); // 3
        tracker.OnLevelEnd(); // 조건 평가
    }

    /// <summary>
    /// 콤보 테스트 (콤보 10으로 전달)
    /// </summary>
    private void TriggerComboTest()
    {
        Debug.Log("[DebugUI] 콤보 테스트 실행");
        tracker.OnComboChanged(10); // 10콤보로 체크
    }

    /// <summary>
    /// 스테이지 클리어 테스트
    /// </summary>
    private void TriggerStageClearTest()
    {
        Debug.Log("[DebugUI] 스테이지 클리어 테스트 실행");
        tracker.OnStageClear();
    }

    /// <summary>
    /// 달성한 도전과제 리스트 출력
    /// </summary>
    private void TriggerListDisplay()
    {
        Debug.Log("[DebugUI] 도전과제 리스트 출력");
        var unlocked = AchievementManager.Instance?.GetUnlockedList();
        if (unlocked != null)
        {
            uiView.ShowList(unlocked);
        }
    }

    #endregion
}
