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

    public enum ItemStatType
    {
        RCL,
        HDL,
        STP,
        SPD,
        DMG,
        MaxBullet,
        Parts
    }

    public enum RankType
    {
        S,
        A,
        B,
        C,
        F
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
    #endregion
    
    #region Interface
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
}