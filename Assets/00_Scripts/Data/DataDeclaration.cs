using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DataDeclaration
{
    #region Enum

    public enum Scene
    {
        Start,
        Lobby,
        Stage
    }

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

    #endregion

    #region Struct

    public struct UserData
    {
        public string AccessToken;
        public string UserID;
        public string UserName;
    }
    
    [Serializable]
    public struct RewardCard
    {
        public Image rewardImage;
        public TextMeshProUGUI rewardName;
        public TextMeshProUGUI timeCost;
        public Button rewardButton;
    }

    #endregion
    
    #region Constants

    public static class Constants
    {
        public const float QUICK_SHOT_TIME = 2f;

        public const int MIN_STAT = 0;
        public const int MAX_STAT = 99;
        
        public const string API_URL =
            "https://script.google.com/macros/s/AKfycbxaG8rWBKYvGxUSD8uFFj-YJDcP4rCbrJj_PO_h_XHwbtDq7-U1jzwfBwIAgDPt7oSX/exec"; // 배포 URL 입력
        
        public const string Google_Sheet_URL =
            "https://docs.google.com/spreadsheets/d/15bfHld_mkidiNNKJLPD6tfa6-ThFvU-kZK76TDynleA"; // 적용할 구글 시트 주소에서 /edit 전까지 입력
    }
    
    #endregion
}