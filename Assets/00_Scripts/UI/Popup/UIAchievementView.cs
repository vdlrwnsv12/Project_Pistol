using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 도전과제 UI를 담당하는 클래스. 팝업 및 리스트 출력 기능을 포함합니다.
/// </summary>
public class UIAchievementView : MonoBehaviour
{
    #region Popup Fields

    [Header("도전과제 팝업 UI")]
    [Tooltip("도전과제 팝업 전체 Panel")]
    [SerializeField] private GameObject popupPanel;

    [Tooltip("팝업에 표시될 텍스트 (제목 + 설명)")]
    [SerializeField] private Text popupText;

    [Tooltip("팝업 자동 종료 시간 (초)")]
    [SerializeField] private float popupDuration = 3f;

    #endregion

    #region List Fields

    [Header("도전과제 리스트 UI")]
    [Tooltip("도전과제 전체를 담고있는 패널")]
    [SerializeField] private GameObject listPanel;
    [Tooltip("도전과제 아이템을 생성할 부모 트랜스폼")]
    [SerializeField] private Transform listParent;

    [Tooltip("도전과제 프리팹")]
    [SerializeField] private GameObject achievementItemPrefab;

    #endregion

    #region Public Methods

    /// <summary>
    /// 도전과제를 달성했을 때 팝업을 출력합니다.
    /// </summary>
    /// <param name="data">달성된 도전과제 데이터</param>
    public void ShowUnlockedPopup(AchievementSO data)
    {
        if (popupPanel == null || popupText == null)
        {
            Debug.LogError("[UIAchievementView] 팝업 관련 UI가 연결되지 않았습니다.");
            return;
        }

        popupPanel.SetActive(true);
        popupText.text = $"{data.title}\n{data.description}";

        StopAllCoroutines();
        StartCoroutine(HidePopupAfterDelay());
    }

    /// <summary>
    /// 도전과제 리스트를 출력합니다.
    /// </summary>
    /// <param name="unlocked">달성된 도전과제 리스트</param>
    public void ShowList(List<AchievementSO> unlocked)
    {
        Debug.Log($"[UI] 출력한 도전과제 수: {unlocked.Count}");

        if (listParent == null || achievementItemPrefab == null)
        {
            Debug.LogError("[UIAchievementView] 리스트 관련 UI가 연결되지 않았습니다.");
            return;
        }

        foreach (Transform child in listParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var data in unlocked)
        {
            GameObject item = Instantiate(achievementItemPrefab, listParent);
            Text itemText = item.GetComponentInChildren<Text>();

            if (itemText != null)
            {
                itemText.text = data.title;
            }

            Debug.Log($"[UI] 출력됨 : {data.title}");
        }
    }

    /// <summary>
    /// 도전과제 리스트 UI를 켜거나 끕니다.
    /// </summary>
    /// <param name="active">true = 열기 / false = 닫기</param>
    public void ToggleListPanel(bool active)
    {
        if (listPanel != null)
        {
            listPanel.SetActive(active);
        }
    }


    #endregion

    #region Private Methods

    /// <summary>
    /// 일정 시간 후 팝업을 자동으로 숨깁니다.
    /// </summary>
    private IEnumerator HidePopupAfterDelay()
    {
        yield return new WaitForSeconds(popupDuration);
        popupPanel.SetActive(false);
    }

    #endregion
}
