using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 도전과제 달성 시 팝업 전용 UI
/// </summary>
public class UIAchievementPopup : MonoBehaviour
{
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private Text popupText;
    [SerializeField] private float duration = 3f;

    public void ShowPopup(AchievementSO data)
    {
        if (popupPanel == null || popupText == null) return;

        popupPanel.SetActive(true);
        popupText.text = $"{data.title}\n{data.description}";
        StopAllCoroutines();
        StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(duration);
        popupPanel.SetActive(false);
    }
}
