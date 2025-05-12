using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 도전과제 상세 정보 UI
/// </summary>
public class AchievementDetailUI : MonoBehaviour
{
    [SerializeField] private Text titleText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Text categoryText;
    [SerializeField] private Text statusText;

    /// <summary>
    /// 상세 정보 UI 설정
    /// </summary>
    public void SetData(AchievementSO data)
    {
        titleText.text = data.title;
        descriptionText.text = data.description;
        categoryText.text = $"유형: {data.conditionType}";

        bool isUnlocked = AchievementManager.Instance.IsUnlocked(data.id);
        statusText.text = isUnlocked ? "달성됨" : "미달성";
    }
}
