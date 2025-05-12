using System;

/// <summary>
/// [도전과제 진행상황 데이터 테이블]
/// UI에 표시할 도전과제의 정보와 달성 여부를 담는 구조체 역할.
/// - 외부 데이터(Json 등)로부터 파싱하거나 코드 내 리스트로 관리됨
/// </summary>
[Serializable]
public class AchievementData
{
    /// <summary>도전과제 이름</summary>
    public string title;

    /// <summary>도전과제 설명</summary>
    public string description;

    /// <summary>도전과제 카테고리</summary>
    public AchievementCategory category;

    /// <summary>달성 여부</summary>
    public bool isCompleted;
}

/// <summary>
/// 도전과제 카테고리 종류
/// </summary>
public enum AchievementCategory
{
    Combat,
    Exploration,
    Collection,
    Challenge
}
