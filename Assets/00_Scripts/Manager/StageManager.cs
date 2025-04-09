using System.Net.NetworkInformation;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    #region Eunm

    /// <summary>
    /// 스테이지정보
    /// </summary>
    /// <param name="StageState">스테이지 공간 내의 세부정보</param>
    /// <returns></returns>
    public enum StageState
    {
        Waiting, //시작 전
        Playing, //진행 중
        Cleared, //클리어
        Failed, //실패
    }

    private StageState stageState;
    #endregion

    #region parameter

        #region bool
        bool isStageStarted; //스테이지 시작여부
        bool isStageCleared; //스테이지 클리어여부
        bool isStageFailed; //스테이지 실패여부
        bool isGateOpened; //스테이지 문을 열었는지 여부
        bool hasPlayerExited;

        #endregion

        #region float int
        float stageTimeLimit; //스테이지 제한시간
        float remainingtime; //남은시간 추적
        float warningThreshold; //10초 이하일때 경고상태 진입
        int currentStageIndex; //현재 스테이지 출력
        int maxStageCount; //스테이지 최대 카운트수

        #endregion

        #region Text
        TextMeshProUGUI timerText; //타이머 텍스트 UI

    #endregion

    Color warningColor; //시간 임박시 색상 변경

    #endregion

    #region Stage start -> end

    /// <summary>
    /// 스테이지 초기 값
    /// </summary>
    private void InitStage()
    {
        stageState = StageState.Playing;
        isStageStarted = true;
        isStageCleared = false;
        isStageFailed = false;
        isGateOpened = false;
        hasPlayerExited = false;

        remainingtime = stageTimeLimit;

        //TODO
        //SpawnTargets(); //새로운 타겟 생성
    }

    /// <summary>
    /// 스테이지 시작시 설정값 매서드
    /// </summary>
    private void StartStage()
    {
        isStageStarted = true;
    }

    /// <summary>
    /// 스테이지매니저 업데이트
    /// </summary>
    private void Update()
    {
        if (stageState != StageState.Playing) return;
        {
            remainingtime -= Time.deltaTime;
        }

        UpdateTimerUI();

        if (remainingtime <= 0f && !isStageFailed) //시간이 0초라면
        {
            HandleStageFail();
        }

        if(remainingtime <= 0)
        {
            HandleStageFail(); //실패
        }
    }

    [SerializeField] private Animator gateAnimator;
    ///<summary>
    ///문을 여는 매서드
    ///</summary>
    private void OpenGate()
    {
        isGateOpened = true;
        gateAnimator.SetTrigger("Open"); //에니메이션에서 Open 트리거 실행
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isStageCleared || !isGateOpened) 
        {
            return; //클리어 문이 열려야 작동
        }

        if (other.CompareTag("Player")) //플레이어이 태그확인
        {
            hasPlayerExited = true;
            LoadNextStage();
        }
    }

    ///<summary>
    ///다음 스테이지 로드
    /// </summary>
    private void LoadNextStage()
    {
        currentStageIndex++;

        if(currentStageIndex >= maxStageCount)
        {
            Debug.Log("게임종료");
            return;
        }

        InitStage(); //스테이지 리셋
    }

    /// <summary>
    /// 타이머 UI 연동
    /// </summary>
    private void UpdateTimerUI()
    {
        int time = Mathf.CeilToInt(remainingtime);
        timerText.text = time.ToString();

        if (time <= warningThreshold)
        {
            timerText.color = warningColor;
        }
        else
        {
            timerText.color = defaultColor;
        }
    }
    #endregion

    #region Handle

    /// <summary>
    /// 스테이지 클리어 매서드
    /// </summary>
    private void HandleStageClear()
    {
        if (isStageCleared)
        {
            return;
        }

        stageState = StageState.Cleared;
        isStageCleared = true;

        OpenGate(); //문열기 처리
    }

    /// <summary>
    /// 스테이지 실패 매서드
    /// </summary>
    private void HandleStageFail()
    {
        stageState = StageState.Failed; //
        isStageFailed = true; //스테이지 실패함
    }
    #endregion

}