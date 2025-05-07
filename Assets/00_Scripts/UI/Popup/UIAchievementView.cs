using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAchievemenetView : MonoBehaviour
{
    [SerializeField] private Transform listParent;
    [SerializeField] private GameObject achievementItemPrefab;
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private Text popupText;

    public void ShowUnlockedPopup (AchievementData data)
    {
        popupPanel.SetActive (true);
        popupText.text = $"{data.title}\\n{data.description}";
        Invoke(nameof(HidePopup), 3f);
    }

    private void HidePopup()
    {
        popupPanel.SetActive(false);
    }

    public void Showlist(List<AchievementData> unlocked)
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
