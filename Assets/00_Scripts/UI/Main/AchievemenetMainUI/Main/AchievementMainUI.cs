using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 도전과제 UI 메인 컨트롤러 (리스트 + 상세 정보 출력 패널 + 패널 토글)
/// </summary>
public class AchievementMainUI : MonoBehaviour
{
    #region Fields

    [Header("리스트 출력")]
    [SerializeField] private Transform listParent;               // 리스트 부모 (ScrollView Content)
    [SerializeField] private GameObject achievementItemPrefab;   // 도전과제 아이템 프리팹
    [SerializeField] private GameObject listPanel;               // 전체 패널 (열기/닫기용)
    [SerializeField] private GameObject detailPanelPrefab;       // 상세정보 패널 프리팹

    private GameObject currentDetailPanel;
    private AchievementItemUI currentSelectedItem;
    private Dictionary<AchievementItemUI, int> originalSiblingIndices = new();

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

        // 기존 리스트 제거
        foreach (Transform child in listParent)
        {
            Destroy(child.gameObject);
        }

        // 도전과제 항목 생성
        foreach (var data in currentAchievements)
        {
            GameObject itemGO = Instantiate(achievementItemPrefab, listParent);
            AchievementItemUI itemUI = itemGO.GetComponent<AchievementItemUI>();
            itemUI.Initialize(data, this); // 데이터 및 MainUI 연결
        }

        // 초기 선택 없음 (모두 닫힘 상태)
        currentSelectedItem = null;
    }

    /// <summary>
    /// 도전과제 항목 클릭 시 호출
    /// </summary>
    /// <param name="clickedItem">클릭된 항목</param>
    public void OnClickAchievementItem(AchievementItemUI clickedItem)
    {
        // 같은 항목 다시 클릭 시 → 닫기
        if (currentSelectedItem == clickedItem)
        {
            CloseDetail();
            return;
        }

        // 기존 열려있는 항목 복원
        if (currentSelectedItem != null)
        {
            RestorePreviousItemPosition();
        }

        // 클릭된 항목 최상단 이동
        SaveOriginalSiblingIndex(clickedItem);
        clickedItem.transform.SetSiblingIndex(0);

        // 상세 패널 생성 또는 활성화
        if (currentDetailPanel == null)
        {
            currentDetailPanel = Instantiate(detailPanelPrefab, listParent);
        }

        currentDetailPanel.SetActive(true);

        // 상세 패널을 클릭 항목 아래로 이동
        int detailIndex = clickedItem.transform.GetSiblingIndex() + 1;
        currentDetailPanel.transform.SetSiblingIndex(detailIndex);

        // 상세 UI 데이터 설정
        var detailUI = currentDetailPanel.GetComponent<AchievementDetailUI>();
        detailUI.SetData(clickedItem.AchievementData);

        currentSelectedItem = clickedItem;
    }

    /// <summary>
    /// 상세정보 닫고 위치 복원
    /// </summary>
    private void CloseDetail()
    {
        if (currentDetailPanel != null)
        {
            currentDetailPanel.SetActive(false);
        }

        RestorePreviousItemPosition();
        currentSelectedItem = null;
    }

    /// <summary>
    /// 원래 위치 기억
    /// </summary>
    private void SaveOriginalSiblingIndex(AchievementItemUI item)
    {
        if (!originalSiblingIndices.ContainsKey(item))
        {
            originalSiblingIndices[item] = item.transform.GetSiblingIndex();
        }
    }

    /// <summary>
    /// 원래 위치로 복구
    /// </summary>
    private void RestorePreviousItemPosition()
    {
        if (currentSelectedItem == null)
        {
            return;
        }

        if (originalSiblingIndices.TryGetValue(currentSelectedItem, out int originalIndex))
        {
            currentSelectedItem.transform.SetSiblingIndex(originalIndex);
            originalSiblingIndices.Remove(currentSelectedItem);
        }
    }

    /// <summary>
    /// 리스트 패널 토글
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
