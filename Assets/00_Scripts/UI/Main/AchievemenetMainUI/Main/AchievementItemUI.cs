using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 도전과제 항목 UI (리스트 내 개별 항목)
/// </summary>
public class AchievementItemUI : MonoBehaviour
{
    #region Fields

    [SerializeField] private Text titleText;                  // 도전과제 제목 텍스트

    public AchievementSO AchievementData { get; private set; }

    private AchievementMainUI mainUI;

    #endregion

    #region Public Methods

    /// <summary>
    /// 도전과제 항목 초기화
    /// </summary>
    /// <param name="data">도전과제 데이터</param>
    /// <param name="parentUI">부모 UI 컨트롤러</param>
    public void Initialize(AchievementSO data, AchievementMainUI parentUI)
    {
        AchievementData = data;
        mainUI = parentUI;
        titleText.text = data.title;

        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// 항목 클릭 시 처리
    /// </summary>
    private void OnClick()
    {
        mainUI.OnClickAchievementItem(this);
    }

    #endregion
}
