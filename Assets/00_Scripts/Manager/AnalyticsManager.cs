using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;

public class AnalyticsManager : SingletonBehaviour<AnalyticsManager>
{
    private async void Awake()
    {
        base.Awake();
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
    }

    public void EndGame()
    {
        Debug.Log("호출");
        // 예시 데이터
    
        float remainTime = StageManager.Instance.RemainTime;
        int stageIndex = RoomManager.Instance.CurStageIndex;
        int roomIndex = RoomManager.Instance.CurRoomIndex;
        string character = GameManager.Instance.selectedCharacter.ToString();
        string weapon = GameManager.Instance.selectedWeapon.ToString();
        int score = StageManager.Instance.GameScore;
        // 커스텀 이벤트 전송
        AnalyticsService.Instance.CustomData("OverZone1", new Dictionary<string, object>
        {
            {nameof(remainTime), remainTime},
            {nameof(stageIndex), stageIndex}, // 스테이지 번호
            {nameof(roomIndex), roomIndex},   // 방 번호 
            {nameof(character), character}, // 캐릭터
            {nameof(weapon), weapon},    // 무기
            {nameof(score),score}
        });

        AnalyticsService.Instance.Flush(); // 데이터 전송 강제 트리거 (선택)
    }

}
