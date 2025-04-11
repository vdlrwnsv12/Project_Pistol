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
        public float rclValue;
        public float hdlValue;
        public float stpValue;
        public float spdValue;
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
}