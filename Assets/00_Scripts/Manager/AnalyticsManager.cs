using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Threading.Tasks;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.TextCore.Text;

public class AnalyticsManager : SingletonBehaviour<AnalyticsManager>
{
  
    public float stageCleartimer = 0f; // 스테이지별 클리어시간
    public float elapsedTime = 0f; // 소요된 시간
                                   // 스테이지별


    #region Room
    public int ShotCount; // 총 쏜 횟수
    public float clearRoomTimer;  // 방 클리어 시간
    public float headShotRoomAccuracy; // 방 헤드샷 명중률 
    public float shotRoomAccuracy; // 방 명중률
    public int roomCombo; // 방 누적 콤보
    public int roomScore; // 방 누적 점수
    #endregion
    private async void Start()
    {
        await InitAnalyticsAsync();


    }
    protected override async void Awake()
    {
        try
        {
            base.Awake();
            await UnityServices.InitializeAsync();
            AnalyticsService.Instance.StartDataCollection();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            //throw; // TODO 예외 처리
        }
    }

    private async Task InitAnalyticsAsync()
    {
        try
        {
            await UnityServices.InitializeAsync();
            AnalyticsService.Instance.StartDataCollection();
            Debug.Log("Analytics initialized.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Unity Services 초기화 실패: {e.Message}");
        }
    }

    public void RoomAnalytics()
    {
        //float roomHeadShotAccuracy = StageManager.Instance.headShotAccuracy;
        //float roomShotAccuracy = StageManager.Instance.shotAccuracy;
        // float roomScore = 
        //AnalyticsService.Instance.CustomData("OverZone1", new Dictionary<string, object>
        //{
        //    {nameof(roomHeadShotAccuracy), roomHeadShotAccuracy}, // 룸별 헤드샷 명중률
        //    {nameof(roomShotAccuracy), roomShotAccuracy},    // 룸별 명중률
        //});

        AnalyticsService.Instance.Flush();
    }
    public void SelectData() // 무기, 캐릭터 선택
    {
        string character = GameManager.Instance.selectedCharacter.ToString();
        string weapon = GameManager.Instance.selectedWeapon.ToString();
        AnalyticsService.Instance.CustomData("OverZone1", new Dictionary<string, object>
        {
            {nameof(character), character}, // 캐릭터
            {nameof(weapon), weapon},    // 무기
        });
        AnalyticsService.Instance.Flush();
    }
    public void ClearGame() // 클리어 성공시
    {
        float remainTime = StageManager.Instance.RemainTime;
        int stageIndex = RoomManager.Instance.CurStageIndex;
        int roomIndex = RoomManager.Instance.CurRoomIndex;
        int score = StageManager.Instance.GameScore;
        float accumulationShotAccuracy = StageManager.Instance.shotAccuracy;
        float accumulationHeadShotAccuracy = StageManager.Instance.headShotAccuracy;
        // 커스텀 이벤트 전송
        AnalyticsService.Instance.CustomData("OverZone1", new Dictionary<string, object>
        {
            {nameof(remainTime), remainTime},
            {nameof(stageIndex), stageIndex}, // 스테이지 번호
            {nameof(roomIndex), roomIndex},   // 방 번호 
            {nameof(score),score}, // 점수
            {nameof(accumulationShotAccuracy),accumulationShotAccuracy }, // 누적 명중률
            {nameof(accumulationHeadShotAccuracy),accumulationHeadShotAccuracy} // 누적 헤드샷 명중률
        });

        AnalyticsService.Instance.Flush(); // 데이터 전송 강제 트리거
    }

    public void FailedGame() // 클리어 실패시 
    {
        int FailedStageIndex = RoomManager.Instance.CurStageIndex;
        int FailedRoomIndex = RoomManager.Instance.CurRoomIndex;
        AnalyticsService.Instance.CustomData("OverZone1", new Dictionary<string, object>
        {
            {nameof(FailedStageIndex) ,FailedStageIndex},
            {nameof(FailedRoomIndex) ,FailedRoomIndex}, 
        });
        AnalyticsService.Instance.Flush(); // 데이터 전송 강제 트리거
    }

    public void RoomInitData() // 방 진입시 데이터 0으로 초기화
    {
        clearRoomTimer = 0;
        ShotCount = 0;
        roomCombo = 0;
        roomScore = 0;
        headShotRoomAccuracy = 0;
        shotRoomAccuracy = 0;
    }

}
