using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DataDeclaration
{
    #region Enum

    public enum MainUIType
    {
        None,
        Start,
        HUD,
        Result,
        Lobby
    }

    public enum RankType
    {
        S,
        A,
        B,
        C,
        F
    }

    public enum ItemApplyType
    {
        Player = 1,
        Weapon
    }

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

    #endregion

    #region Struct

    [Serializable]
    public struct RewardCard
    {
        public Image rewardImage;
        public TextMeshProUGUI rewardName;
        public TextMeshProUGUI timeCost;
        public Button rewardButton;
    }

    [Serializable]
    public struct CharacterStat
    {
        private float rclValue;
        private float hdlValue;
        private float stpValue;
        private float spdValue;

        public float RCL
        {
            get => rclValue;
            set => rclValue = Mathf.Clamp(value, 0, Constant.MAX_STAT);
        }

        public float HDL
        {
            get => hdlValue;
            set => hdlValue = Mathf.Clamp(value, 0, Constant.MAX_STAT);
        }
        public float STP
        {
            get => stpValue;
            set => stpValue = Mathf.Clamp(value, 0, Constant.MAX_STAT);
        }
        public float SPD
        {
            get => spdValue;
            set => spdValue = Mathf.Clamp(value, 0, Constant.MAX_STAT);
        }
    }

    #endregion

    #region Interface

    public interface IState // 앞으로 모든 상태들은 IState를 상속 받는다
    {
        public void Enter();
        public void Exit();
        public void HandleInput(); // 입력받는거 핸들링
        public void Update();
        public void PhysicsUpdate(); // 물리 중력
    }

    //TODO: 사용X, 수정해야함
    /// <summary>
    /// ObjectPooling을 위한 Interface
    /// </summary>
    public interface IPoolable
{
    GameObject GameObject { get; }  // 게임 오브젝트
    int ResourceInstanceID { get; } // 리소스의 고유 Instance ID
    bool IsAutoReturn { get; }      // 자동 반환 여부
    float ReturnTime { get; }       // 자동 반환 대기 시간
}


    #endregion

    public static class Constant
    {
        public const float MAX_STAT = 99f;
    }
}