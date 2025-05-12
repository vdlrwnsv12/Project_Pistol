using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 도전과제 UI 메인 컨트롤러 (리스트 + 상세 정보 출력 + 패널 토글)
/// </summary>
public class AchievementMainUI : MonoBehaviour
{
    #region Fields

    [Header("리스트 출력")]
    [SerializeField] private Transform listParent;               // 리스트 부모 (Content)
    [SerializeField] private GameObject achievementItemPrefab;   // 도전과제 아이템 프리팹
    [SerializeField] private GameObject listPanel;               // 리스트 전체 패널 (토글용)

    [Header("상세 정보 출력")]
    [SerializeField] private Text titleText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Text statusText;
    [SerializeField] private Text categoryText;

    private List<AchievementSO> currentAchievements;

    #endregion

    #region Public Methods

    /// <summary>
    /// 도전과제 리스트 생성 및 출력
    /// </summary>
    /// <param name="achievements">도전과제 목록</param>
    public void ShowList(List<AchievementSO> achievements)
    {
        currentAchievements = achievements;

        foreach (Transform child in listParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var data in currentAchievements)
        {
            GameObject item = Instantiate(achievementItemPrefab, listParent);
            item.GetComponentInChildren<Text>().text = data.title;

            Button btn = item.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(() => ShowDetail(data));
            }
        }

        if (currentAchievements.Count > 0)
        {
            ShowDetail(currentAchievements[0]); // 첫 항목 상세 표시
        }
    }

    /// <summary>
    /// 도전과제 상세 정보 출력
    /// </summary>
    /// <param name="data">선택된 도전과제</param>
    public void ShowDetail(AchievementSO data)
    {
        titleText.text = data.title;
        descriptionText.text = data.description;

        bool isUnlocked = AchievementManager.Instance.IsUnlocked(data.id);
        statusText.text = isUnlocked ? "달성됨" : "미달성";
        categoryText.text = $"유형: {data.conditionType}";
    }

    /// <summary>
    /// 도전과제 리스트 패널 열기/닫기 토글
    /// </summary>
    public void ToggleListPanel()
    {
        if (listPanel != null)
        {
            bool isActive = !listPanel.activeSelf;
            listPanel.SetActive(isActive);
            Debug.Log($"[AchievementMainUI] 리스트 패널 토글: {isActive}");
        }
        else
        {
            Debug.LogWarning("[AchievementMainUI] listPanel이 연결되어 있지 않습니다.");
        }
    }

    #endregion
}
