using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 도전과제 상세 정보 UI
/// </summary>
public class AchievementDetailUI : MonoBehaviour
{
    #region Fields

    [SerializeField] private Text titleText;          // 도전과제 제목
    [SerializeField] private Text descriptionText;    // 도전과제 설명
    [SerializeField] private Text categoryText;       // 도전과제 유형
    [SerializeField] private Text statusText;         // 달성 여부

    #endregion

    #region Public Methods

    /// <summary>
    /// 상세 정보 UI에 데이터 표시
    /// </summary>
    /// <param name="data">도전과제 데이터</param>
    public void SetData(AchievementSO data)
    {
        titleText.text = data.title;
        descriptionText.text = data.description;
        categoryText.text = $"유형: {data.conditionType}";

        bool isUnlocked = AchievementManager.Instance.IsUnlocked(data.id);
        statusText.text = isUnlocked ? "달성됨" : "미달성";
    }

    #endregion
}
