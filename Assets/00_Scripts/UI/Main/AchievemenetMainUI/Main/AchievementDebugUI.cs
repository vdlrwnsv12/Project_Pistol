using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 도전과제 디버그 UI (조건 강제 등록용 버튼)
/// </summary>
public class AchievementDebugUI : MonoBehaviour
{
    #region Fields

    [Tooltip("도전과제 UI 프리팹을 Initialize합니다")]
    [SerializeField] private AchievementUIPrefabSet achievementUIPrefabSet;

    [Header("디버그 버튼")]

    [Tooltip("일반 킬 1회를 등록합니다.")]
    [SerializeField] private Button killButton;

    [Tooltip("헤드샷 킬 1회를 등록합니다.")]
    [SerializeField] private Button headshotButton;

    [Tooltip("550m 이상 거리에서의 헤드샷 킬을 등록합니다.")]
    [SerializeField] private Button longshotButton;

    [Tooltip("명중 판정을 1회 등록합니다. (연속 명중 카운트 증가)")]
    [SerializeField] private Button hitButton;

    [Tooltip("미스를 등록하여 연속 명중 수를 초기화합니다.")]
    [SerializeField] private Button missButton;

    [Tooltip("5~15 범위의 랜덤 콤보 수치를 등록합니다.")]
    [SerializeField] private Button comboButton;

    [Tooltip("스테이지 클리어 시간을 랜덤으로 등록합니다.")]
    [SerializeField] private Button stageClearButton;

    [Tooltip("도전과제 리스트 패널을 열거나 닫습니다.")]
    [SerializeField] private Button showListButton;

    [Header("연동된 시스템")]

    [Tooltip("실시간 스탯을 추적하는 PlayerStatTracker 참조")]
    [SerializeField] private PlayerStatTracker tracker;

    [Tooltip("도전과제 UI 메인 컨트롤러 참조")]
    [SerializeField] private AchievementMainUI uiMain;

    #endregion

    #region Unity Methods

    private void Start()
    {

        if (tracker == null || uiMain == null)
        {
            Debug.LogError("[AchievementDebugUI] 트래커 또는 UIMain이 연결되지 않았습니다.");
            return;
        }

        killButton.onClick.AddListener(() =>
        {
            tracker.RegisterKill(false, 10f, "Rifle");
            Debug.Log("[디버그] 일반 킬 1회 등록됨.");
        });

        headshotButton.onClick.AddListener(() =>
        {
            tracker.RegisterKill(true, 10f, "Rifle");
            Debug.Log("[디버그] 헤드샷 킬 1회 등록됨.");
        });

        longshotButton.onClick.AddListener(() =>
        {
            tracker.RegisterKill(true, 550f, "Sniper");
            Debug.Log("[디버그] 550m 거리 헤드샷 킬 등록됨.");
        });

        hitButton.onClick.AddListener(() =>
        {
            tracker.RegisterHit();
            Debug.Log("[디버그] 명중 1회 등록됨.");
        });

        missButton.onClick.AddListener(() =>
        {
            tracker.RegisterMiss();
            Debug.Log("[디버그] 미스 등록됨 (연속 명중 초기화).");
        });

        comboButton.onClick.AddListener(() =>
        {
            int combo = Random.Range(5, 15);
            tracker.RegisterCombo(combo);
            Debug.Log($"[디버그] 콤보 {combo}회 등록됨.");
        });

        stageClearButton.onClick.AddListener(() =>
        {
            float time = Random.Range(30f, 100f);
            tracker.RegisterClear(time);
            Debug.Log($"[디버그] 스테이지 클리어 등록됨 (소요 시간: {time:F1}초).");
        });

        showListButton.onClick.AddListener(() =>
        {
            Debug.Log("[디버그] 도전과제 리스트 패널 호출됨.");
            AchievementSystemLauncher.ShowAchievementUI();
        });
    }

    #endregion
}
