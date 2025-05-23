using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyAchievementItem : MonoBehaviour
{
    public AchievementSO achievementSO;

    [SerializeField] private Text titleText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private GameObject badge;
    [SerializeField] private Slider slider;


    /// <summary>
    /// 도전과제 정보 및 달성률 세팅
    /// </summary>
    public void InitData()
    {
        float rate = AchievementManager.Instance.GetAchievementRate(achievementSO);
        slider.value = rate;

        if (rate >= 1f)
        {
            badge.SetActive(true);
        }

        titleText.text = achievementSO.Name;
        descriptionText.text = achievementSO.Description;
    }
}
