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
    [SerializeField] private AchievementMainUI uiMain;

    #endregion

    #region Unity Methods

    private void Start()
    {
        if (tracker == null || uiMain == null)
        {
            Debug.LogError("트래커 또는 UI가 연결되지 않았습니다.");
            return;
        }

        if (headshotButton != null)
            headshotButton.onClick.AddListener(() =>
            {
                tracker.OnHeadshot();  // 헤드샷 처리
                tracker.OnShotFired();
                tracker.OnLevelEnd();  // 헤드샷 비율 체크
            });

        if (comboButton != null)
            comboButton.onClick.AddListener(() =>
            {
                tracker.OnComboChanged(10);  // 콤보 테스트
            });

        if (stageClearButton != null)
            stageClearButton.onClick.AddListener(() =>
            {
                tracker.OnStageClear();  // 스테이지 클리어
            });

        if (showListButton != null)
            showListButton.onClick.AddListener(() =>
            {
                var list = AchievementManager.Instance.GetUnlockedList();
                uiMain.ShowList(list);  // UI에 리스트 출력
            });
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
    /// 도전과제 리스트 UI 토글
    /// </summary>
    private void TriggerListDisplay()
    {
        Debug.Log("[DebugUI] 도전과제 리스트 열기/닫기 토글");

        var unlocked = AchievementManager.Instance?.GetUnlockedList();
        if (unlocked != null)
        {
            uiMain.ShowList(unlocked);
            uiMain.ToggleListPanel();  // 리스트 패널 열고 닫기
        }
    }

    #endregion
}
