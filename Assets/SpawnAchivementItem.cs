using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnAchivementItem : SingletonBehaviour<SpawnAchivementItem>
{
    [SerializeField] private GameObject achivementItem;
    [SerializeField] private Transform achivementSpawnPoint;
    [SerializeField] private AchivementDataContainer achivementDataContainer;

    /// <summary>
    /// 도전과제 띄우는
    /// </summary>
    public void ShowAchivementEffect(string achievementSOID)
    {
        // ResourceManager로 SO 로드
        var achievementSO = ResourceManager.Instance.Load<AchievementSO>($"Data/SO/AchievementSO/{achievementSOID}");
        if (achievementSO == null)
        {
            Debug.LogError($"AchievementSO {achievementSOID} 을(를) 못 불러옴");
            return;
        }

        SpawnAchivementTexts(achievementSO.Name, achievementSO.Description);
    }

    void SpawnAchivementTexts(string titleText, string descriptionText)
    {
        // ResourceManager로 프리팹 로드
        var prefab = ResourceManager.Instance.Load<GameObject>("Prefabs/UI/AchivementItem");
        if (prefab == null)
        {
            Debug.LogError("AchivementItem 프리팹을 못 불러옴");
            return;
        }

        var go = ObjectPoolManager.Instance.GetObject(prefab, achivementSpawnPoint.position, Quaternion.identity, 2f);

        go.transform.SetParent(UIManager.Instance.CurMainUI.transform, false);
        go.transform.localRotation = Quaternion.identity;

        var dataContainer = go.GetComponent<AchivementDataContainer>();
        if (dataContainer == null)
        {
            Debug.LogError("AchivementDataContainer가 없음");
            return;
        }

        dataContainer.titleText.text = titleText;
        dataContainer.achiveText.text = descriptionText;
    }
}