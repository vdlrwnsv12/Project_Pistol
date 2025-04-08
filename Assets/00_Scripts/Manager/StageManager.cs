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

    bool isStageStarted; //스테이지 시작여부
    bool isStageCleared; //스테이지 클리어여부
    bool isStageFailed; //스테이지 실패여부
    bool isGateOpened; //스테이지 문을 열었는지 여부
    bool hasPlayerExited;

    float stageTimeLimit; //스테이지 제한시간
    float remainingtime; //남은시간 추적
    int currentStageIndex; //현재 스테이지 출력
    #endregion


    /// <summary>
    /// 스테이지 시작시 설정값 매서드
    /// </summary>
    private void StartStage()
    {
        isStageStarted = true;
    }

    private void InitStage()
    {
        stageState = StageState.Playing;
        isStageStarted = true;
        isStageCleared = false;
        isStageFailed = false;
        remainingtime = stageTimeLimit;
    }

    /// <summary>
    /// 스테이지매니저 업데이트
    /// </summary>
    private void Update()
    {
        if (stageState != StageState.Playing) return; 

        if (remainingtime <= 0f && !isStageFailed) //시간이 0초라면

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
