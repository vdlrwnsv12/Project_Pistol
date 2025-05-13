using DG.Tweening;
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

    [Header("버튼 컴포넌트")]
    [SerializeField] private Button closeButton; // 닫기 버튼

    private GameObject currentDetailPanel;
    private CanvasGroup detailCanvasGroup;
    private AchievementItemUI currentSelectedItem;
    private Dictionary<AchievementItemUI, int> originalSiblingIndices = new();

    private List<AchievementSO> currentAchievements;

    #endregion

    #region Start

    private void Start()
    {
        closeButton.onClick.AddListener(CloseUI);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// 도전과제 리스트 생성 및 출력
    /// </summary>
    /// <param name="achievements">도전과제 리스트</param>
    public void ShowList(List<AchievementSO> achievements)
    {
        currentAchievements = achievements;

        foreach (Transform child in listParent)
        {
            Destroy(child.gameObject);
        }

        originalSiblingIndices.Clear();

        foreach (var data in currentAchievements)
        {
            GameObject itemGO = Instantiate(achievementItemPrefab, listParent);
            AchievementItemUI itemUI = itemGO.GetComponent<AchievementItemUI>();
            itemUI.Initialize(data, this);
            originalSiblingIndices[itemUI] = itemGO.transform.GetSiblingIndex();
        }

        currentSelectedItem = null;
    }

    /// <summary>
    /// 도전과제 항목 클릭 시 호출
    /// </summary>
    /// <param name="clickedItem">클릭된 도전과제 항목</param>
    public void OnClickAchievementItem(AchievementItemUI clickedItem)
    {
        if (currentSelectedItem == clickedItem)
        {
            AnimateDetailClose();
            return;
        }

        if (currentSelectedItem != null)
        {
            RestorePreviousItemPosition();
        }

        SaveOriginalSiblingIndex(clickedItem);
        clickedItem.transform.SetSiblingIndex(0);

        RectTransform itemRect = clickedItem.GetComponent<RectTransform>();
        itemRect.DOAnchorPosY(-45f, 0.3f).SetEase(Ease.OutCubic);

        if (currentDetailPanel == null)
        {
            currentDetailPanel = Instantiate(detailPanelPrefab, listParent);
            detailCanvasGroup = currentDetailPanel.GetComponent<CanvasGroup>();
        }

        int detailIndex = clickedItem.transform.GetSiblingIndex() + 1;
        currentDetailPanel.transform.SetSiblingIndex(detailIndex);

        var detailUI = currentDetailPanel.GetComponent<AchievementDetailUI>();
        detailUI.SetData(clickedItem.AchievementData);

        AnimateDetailOpen();

        currentSelectedItem = clickedItem;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// 기존 항목의 SiblingIndex 저장
    /// </summary>
    private void SaveOriginalSiblingIndex(AchievementItemUI item)
    {
        if (!originalSiblingIndices.ContainsKey(item))
        {
            originalSiblingIndices[item] = item.transform.GetSiblingIndex();
        }
    }

    /// <summary>
    /// 항목 원래 자리로 복원
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
        }
    }

    /// <summary>
    /// 상세정보 열기 애니메이션 실행
    /// </summary>
    private void AnimateDetailOpen()
    {
        currentDetailPanel.SetActive(true);

        RectTransform rect = currentDetailPanel.GetComponent<RectTransform>();
        detailCanvasGroup.alpha = 0f;
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, -50f);

        detailCanvasGroup.DOFade(1f, 0.3f);
        rect.DOAnchorPosY(-650f, 0.3f).SetEase(Ease.OutCubic);
    }

    /// <summary>
    /// 상세정보 닫기 애니메이션 실행 및 복원
    /// </summary>
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

    /// <summary>
    /// 도전과제 패널 열기/닫기 토글
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

    /// <summary>
    /// 도전과제 메인 UI 전체를 닫는다
    /// </summary>
    private void CloseUI()
    {
        listPanel.SetActive(false);
    }


    #endregion
}
