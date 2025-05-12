using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
    private CanvasGroup detailCanvasGroup;
    private AchievementItemUI currentSelectedItem;
    private Dictionary<AchievementItemUI, int> originalSiblingIndices = new();

    private List<AchievementSO> currentAchievements;

    #endregion

    #region Public Methods

    /// <summary>
    /// 도전과제 리스트 생성 및 출력
    /// </summary>
    public void ShowList(List<AchievementSO> achievements)
    {
        currentAchievements = achievements;

        // 기존 리스트 제거
        foreach (Transform child in listParent)
        {
            Destroy(child.gameObject);
        }

        originalSiblingIndices.Clear();

        // 도전과제 항목 생성
        foreach (var data in currentAchievements)
        {
            GameObject itemGO = Instantiate(achievementItemPrefab, listParent);
            AchievementItemUI itemUI = itemGO.GetComponent<AchievementItemUI>();
            itemUI.Initialize(data, this);

            // 초기 위치 기억
            originalSiblingIndices[itemUI] = itemGO.transform.GetSiblingIndex();
        }

        currentSelectedItem = null;
    }

    /// <summary>
    /// 도전과제 항목 클릭 시 호출
    /// </summary>
    public void OnClickAchievementItem(AchievementItemUI clickedItem)
    {
        if (currentSelectedItem == clickedItem)
        {
            AnimateDetailClose();
            return;
        }

        // 이전 항목 복원
        if (currentSelectedItem != null)
        {
            RestorePreviousItemPosition();
        }

        // 위치 기억 & 최상단 이동
        SaveOriginalSiblingIndex(clickedItem);
        clickedItem.transform.SetSiblingIndex(0);

        // 슬라이드 애니메이션 (항목)
        RectTransform itemRect = clickedItem.GetComponent<RectTransform>();
        itemRect.DOAnchorPosY(-45f, 0.3f).SetEase(Ease.OutCubic);

        // 상세 패널 생성 (최초)
        if (currentDetailPanel == null)
        {
            currentDetailPanel = Instantiate(detailPanelPrefab, listParent);
            detailCanvasGroup = currentDetailPanel.GetComponent<CanvasGroup>();
        }

        // 위치 조정
        int detailIndex = clickedItem.transform.GetSiblingIndex() + 1;
        currentDetailPanel.transform.SetSiblingIndex(detailIndex);

        // 데이터 표시
        var detailUI = currentDetailPanel.GetComponent<AchievementDetailUI>();
        detailUI.SetData(clickedItem.AchievementData);

        // 애니메이션
        AnimateDetailOpen();

        currentSelectedItem = clickedItem;
    }

    #endregion

    #region Private Methods

    private void SaveOriginalSiblingIndex(AchievementItemUI item)
    {
        if (!originalSiblingIndices.ContainsKey(item))
        {
            originalSiblingIndices[item] = item.transform.GetSiblingIndex();
        }
    }

    private void RestorePreviousItemPosition()
    {
        if (currentSelectedItem == null)
        {
            return;
        }

        if (originalSiblingIndices.TryGetValue(currentSelectedItem, out int originalIndex))
        {
            currentSelectedItem.transform.SetSiblingIndex(originalIndex);
        }
    }

    private void AnimateDetailOpen()
    {
        currentDetailPanel.SetActive(true);

        RectTransform rect = currentDetailPanel.GetComponent<RectTransform>();
        detailCanvasGroup.alpha = 0f;
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, -50f);

        detailCanvasGroup.DOFade(1f, 0.3f);
        rect.DOAnchorPosY(-650f, 0.3f).SetEase(Ease.OutCubic);  //최종 상세정보 위치 보정
    }

    private void AnimateDetailClose()
    {
        if (currentDetailPanel == null) return;

        RectTransform rect = currentDetailPanel.GetComponent<RectTransform>();

        Sequence seq = DOTween.Sequence();
        seq.Append(detailCanvasGroup.DOFade(0f, 0.2f));
        seq.Join(rect.DOAnchorPosY(-50f, 0.2f));
        seq.OnComplete(() =>
        {
            currentDetailPanel.SetActive(false);
            RestorePreviousItemPosition();
            currentSelectedItem = null;
        });
    }

    #endregion

    #region UI Controls

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
