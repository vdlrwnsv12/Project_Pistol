using DataDeclaration;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    #region Singleton

    private static StageManager instance;

    public static StageManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<StageManager>();
                if (instance == null)
                {
                    instance = FindAnyObjectByType<StageManager>();
                    if (instance == null)
                    {
                        var go = new GameObject
                        {
                            name = nameof(StageManager)
                        };
                        instance = go.AddComponent<StageManager>();
                    }
                }
            }

            return instance;
        }
    }

    #endregion
    
    private StageState stageState;

    #region Parameters

    [Header("스테이지 데이터")]
    [SerializeField] private ItemSO[] stageDataArray; // 각 스테이지별 설정값 목록

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

    [Header("애니메이션")]
    [SerializeField] private Animator gateAnimator;     // 문 애니메이터
    
    public RewardSystem RewardSystem { get; private set; }
    public ScoreSystem ScoreSystem { get; private set; }

    private HUDUI hudUI;

    #endregion

    #region Stage Flow

    /// <summary>
    /// 스테이지 초기화 (데이터 적용 및 상태 초기화)
    /// </summary>
    private void InitStage()
    {
        ItemSO data = stageDataArray[currentStageIndex];

        // stageTimeLimit = data.stageTimeLimit;
        // remainingTime = stageTimeLimit;
        // warningThreshold = data.warningThreshold;
        // defaultColor = data.defaultColor;
        // warningColor = data.warningColor;

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

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        
        RewardSystem = new RewardSystem();
        ScoreSystem = new ScoreSystem();
        
        InitHUDUI();
    }

    private void Update()
    {
        if (stageState != StageState.Playing)
            return;

        remainingTime -= Time.deltaTime;

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

    private void InitHUDUI()
    {
        UIManager.Instance.InitUI<HUDUI>();
        UIManager.Instance.ChangeMainUI(MainUIType.HUD);
        hudUI = UIManager.Instance.CurMainUI as HUDUI;
    }
}
