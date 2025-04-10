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
    [SerializeField] private StageData[] stageDataArray;

    [Header("상태 플래그")]
    private bool isStageStarted;
    private bool isStageCleared;
    private bool isStageFailed;
    private bool isGateOpened;
    private bool hasPlayerExited;

    [Header("타이머 설정")]
    private float stageTimeLimit;
    private float remainingTime;
    private float warningThreshold;

    [Header("스테이지 진행")]
    [SerializeField] private int currentStageIndex;
    private int maxStageCount => stageDataArray.Length;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Color defaultColor;
    private Color warningColor;

    [Header("애니메이션")]
    [SerializeField] private Animator gateAnimator;

    #endregion

    #region Stage Flow

    /// <summary>
    /// 스테이지 초기화
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

        // TODO: SpawnTargets();
    }

    /// <summary>
    /// 스테이지 시작 설정
    /// </summary>
    private void StartStage()
    {
        isStageStarted = true; // TODO: 시작 연출 or UI
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
    /// 문 열기
    /// </summary>
    private void OpenGate()
    {
        isGateOpened = true;
        gateAnimator.SetTrigger("Open");
    }

    /// <summary>
    /// 문 통과 시 다음 스테이지 로드
    /// </summary>
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
    /// 다음 스테이지 로드
    /// </summary>
    private void LoadNextStage()
    {
        currentStageIndex++;

        if (currentStageIndex >= maxStageCount)
        {
            Debug.Log("게임종료");
            return;
        }

        InitStage();
    }

    /// <summary>
    /// 타이머 UI 업데이트
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
    }

    #endregion
}
