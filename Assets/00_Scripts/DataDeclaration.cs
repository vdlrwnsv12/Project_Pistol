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
        HUD,
        Result,
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
        Player,
        Weapon
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
            get => Mathf.Clamp(rclValue, 0, Constant.MAX_STAT);
            set => rclValue = value;
        }
        public float HDL
        {
            get => Mathf.Clamp(hdlValue, 0, Constant.MAX_STAT);
            set => hdlValue = value;
        }
        public float STP
        {
            get => Mathf.Clamp(stpValue, 0, Constant.MAX_STAT);
            set => stpValue = value;
        }
        public float SPD
        {
            get => Mathf.Clamp(spdValue, 0, Constant.MAX_STAT);
            set => spdValue = value;
        }
    }

    #endregion

    #region Interface

    //TODO: 사용X, 수정해야함
    /// <summary>
    /// ObjectPooling을 위한 Interface
    /// </summary>
    public interface IPoolable
    {
        /// <summary>
        /// 리소스의 InstanceID
        /// </summary>
        public int ResourceInstanceID { get; set; }

        /// <summary>
        /// 리소스의 Prefab
        /// </summary>
        public GameObject GameObject { get; }
    }

    #endregion

    public static class Constant
    {
        public const float MAX_STAT = 99f;
    }
}