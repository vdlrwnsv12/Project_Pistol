using TMPro;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    #region Enum

    /// <summary>
    /// 스테이지 상태 정보
    /// </summary>
    public enum StageState
    {
        Waiting, // 시작 전
        Playing, // 진행 중
        Cleared, // 클리어
        Failed   // 실패
    }

    private StageState stageState;

    #endregion

    #region Parameters

    [Header("스테이지 데이터")]
    [SerializeField] private StageData[] stageDataArray; // 각 스테이지별 설정값 목록

    [Header("상태 플래그")]
    private bool isStageStarted;       // 스테이지 시작 여부
    private bool isStageCleared;       // 스테이지 클리어 여부
    private bool isStageFailed;        // 스테이지 실패 여부
    private bool isGateOpened;         // 문이 열렸는지 여부
    private bool hasPlayerExited;      // 플레이어가 탈출했는지 여부

    [Header("타이머 설정")]
    private float stageTimeLimit;      // 스테이지 제한 시간
    private float remainingTime;       // 남은 시간
    private float warningThreshold;    // 경고 색상 전환 임계 시간

    [Header("스테이지 진행")]
    [SerializeField] private int currentStageIndex; // 현재 스테이지 인덱스
    private int maxStageCount => stageDataArray.Length; // 최대 스테이지 수

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timerText; // TODO: 리팩토링 필요 (UI 텍스트 매니저와 분리 여부 검토)
    [SerializeField] private Color defaultColor;        // 기본 타이머 색상
    private Color warningColor;                         // 경고 타이머 색상

    [Header("애니메이션")]
    [SerializeField] private Animator gateAnimator;     // 문 애니메이터

    #endregion

    #region Stage Flow

    /// <summary>
    /// 스테이지 초기화 (데이터 적용 및 상태 초기화)
    /// </summary>
    private void InitStage()
    {
        StageData data = stageDataArray[currentStageIndex];

        stageTimeLimit = data.stageTimeLimit;
        remainingTime = stageTimeLimit;
        warningThreshold = data.warningThreshold;
        defaultColor = data.defaultColor;
        warningColor = data.warningColor;

        stageState = StageState.Playing;
        isStageStarted = true;
        isStageCleared = false;
        isStageFailed = false;
        isGateOpened = false;
        hasPlayerExited = false;

        // TODO: SpawnTargets(); // 타겟 생성 처리 예정
    }

    /// <summary>
    /// 스테이지 시작 설정
    /// </summary>
    private void StartStage()
    {
        InitStage(); // TODO: StartStage 내에서 InitStage 호출 구조 변경 검토 필요
    }

    /// <summary>
    /// 스테이지 진행 업데이트
    /// </summary>
    private void Update()
    {
        if (stageState != StageState.Playing)
            return;

        remainingTime -= Time.deltaTime;
        UpdateTimerUI();

        if (remainingTime <= 0f && !isStageFailed)
        {
            HandleStageFail();
        }
    }

    /// <summary>
    /// 문 열기 처리 (애니메이션 트리거)
    /// </summary>
    private void OpenGate()
    {
        isGateOpened = true;
        gateAnimator.SetTrigger("Open");
    }

    /// <summary>
    /// 문 통과 시 다음 스테이지 로드
    /// </summary>
    /// <param name="other">충돌 대상</param>
    private void OnTriggerEnter(Collider other)
    {
        if (stageState != StageState.Cleared || !isGateOpened)
            return;

        if (other.CompareTag("Player"))
        {
            hasPlayerExited = true;
            LoadNextStage();
        }
    }

    /// <summary>
    /// 다음 스테이지 로드 및 초기화
    /// </summary>
    private void LoadNextStage()
    {
        currentStageIndex++;

        if (currentStageIndex >= maxStageCount)
        {
            Debug.Log("게임종료");
            // TODO: 메인 스테이지 또는 결과 화면으로 전환 로직 추가 필요
            return;
        }

        InitStage();
    }

    /// <summary>
    /// 타이머 UI 갱신
    /// </summary>
    private void UpdateTimerUI()
    {
        int time = Mathf.CeilToInt(remainingTime);
        timerText.text = time.ToString();
        timerText.color = time <= warningThreshold ? warningColor : defaultColor;
    }

    #endregion

    #region Handle

    /// <summary>
    /// 스테이지 클리어 처리
    /// </summary>
    private void HandleStageClear()
    {
        if (isStageCleared)
            return;

        stageState = StageState.Cleared;
        isStageCleared = true;
        OpenGate();
    }

    /// <summary>
    /// 스테이지 실패 처리
    /// </summary>
    private void HandleStageFail()
    {
        stageState = StageState.Failed;
        isStageFailed = true;
        // TODO: 실패 후 리턴 처리 및 메인화면/리트라이 UI 연동 필요
    }

    #endregion
}
