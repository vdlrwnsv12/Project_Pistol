using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 도전과제 UI 메인 컨트롤러
/// 리스트 출력 및 상세 정보 갱신 담당
/// </summary>
public class UIAchievementMainUI : MonoBehaviour
{
    [Header("리스트 출력")]
    [SerializeField] private Transform listParent;               // Content 오브젝트
    [SerializeField] private GameObject achievementItemPrefab;   // 프리팹 (텍스트 + 버튼)

    [Header("상세 정보 출력")]
    [SerializeField] private Text titleText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Text statusText;
    [SerializeField] private Text categoryText;

    private List<AchievementSO> currentAchievements;

    /// <summary>
    /// 도전과제 리스트 생성
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
    /// 상세 정보 패널 갱신
    /// </summary>
    /// <param name="data">선택된 도전과제</param>
    private void ShowDetail(AchievementSO data)
    {
        titleText.text = data.title;
        descriptionText.text = data.description;

        bool isUnlocked = AchievementManager.Instance.IsUnlocked(data.id);
        statusText.text = isUnlocked ? "달성됨" : "미달성";
        categoryText.text = $"유형: {data.conditionType}";
    }
}
