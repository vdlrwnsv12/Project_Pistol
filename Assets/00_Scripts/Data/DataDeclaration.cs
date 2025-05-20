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

    public enum TargetType
    {
        LandTarget,
        AerialTarget,
        CivilianTarget,
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

    public enum WeaponPartsType
    {
        Sight = 1,
        Rail = 2,
        Muzzle = 4,
    }

    #endregion

    #region Struct

    [Serializable]
    public struct UserData
    {
        public string AccessToken;  // 토큰 확인용
        public string UserID;
        public string UserName;

        public int BestScore;
        
        public string SelectedCharacterID;
        public string SelectedWeaponID;
    }

    #endregion
    
    #region Constants

    public static class Constants
    {
        #region User Data

        // Key 값으로 사용할 상수
        // 공백, 한글, 특수문자 금지
        public const string USER_DATA = "User_Data";
        public const string USER_NAME = "User_Name";

        #endregion
        
        public const int LAST_STAGE_INDEX = 8;
        public const int MAX_ROOM_INDEX = 3;

        public const float INIT_STAGE_TIME = 20f;
        public const float ADDITIONAL_STAGE_TIME = 20f;
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