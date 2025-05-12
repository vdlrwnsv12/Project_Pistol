using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 도전과제 항목 UI (리스트 내 개별 항목)
/// </summary>
public class AchievementItemUI : MonoBehaviour
{
    [SerializeField] private Text titleText;

    public AchievementSO AchievementData { get; private set; }

    private AchievementMainUI mainUI;

    /// <summary>
    /// 항목 초기화
    /// </summary>
    public void Initialize(AchievementSO data, AchievementMainUI parentUI)
    {
        AchievementData = data;
        mainUI = parentUI;
        titleText.text = data.title;

        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        mainUI.OnClickAchievementItem(this);
    }
}
