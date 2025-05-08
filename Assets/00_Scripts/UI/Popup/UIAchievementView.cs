using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAchievementView : MonoBehaviour
{
    [Header("리스트 출력")]
    [SerializeField] private Transform listParent;
    [SerializeField] private GameObject achievementItemPrefab;

    [Header("팝업 출력")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private Text popupText;

    public void ShowUnlockedPopup(AchievementSO data)
    {
        popupPanel.SetActive(true);
        popupText.text = $"{data.title}\n{data.description}";
        Invoke(nameof(HidePopup), 3f);
    }

    private void HidePopup()
    {
        popupPanel.SetActive(false);
    }

    public void ShowList(List<AchievementSO> unlocked)
    {
        foreach (Transform child in listParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var data in unlocked)
        {
            var item = Instantiate(achievementItemPrefab, listParent);
            item.GetComponentInChildren<Text>().text = data.title;
        }
    }
}
