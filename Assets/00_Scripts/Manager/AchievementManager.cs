using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class AchievementManager : SingletonBehaviour<AchievementManager>
{
    private Dictionary<string, AchievementProgressData> progressDict = new Dictionary<string, AchievementProgressData>();

    public void AddProgress(string id, float value)
    {
        if (!progressDict.ContainsKey(id))
        {
            progressDict[id] = new AchievementProgressData(id, 0);
        }

        progressDict[id].CurrValue += value;
        ChcekAndDisplayAchievement(id);
    }

    public float GetAchievementRate(AchievementSO so)
    {
        if (!progressDict.ContainsKey(so.ID))
        {
            return 0f;
        }

        return Mathf.Clamp01(progressDict[so.ID].CurrValue / so.AchievementRate);
    }
    public void ChcekAndDisplayAchievement(string id)
    {
        AchievementSO so = ResourceManager.Instance.Load<AchievementSO>($"Data/SO/AchievementSO/{id}");

        float rate = GetAchievementRate(so);
        if (rate >= 1f)
        {
            SpawnAchivement(so.ID);
        }
    }
    private AchivementDataContainer achivementDataContainer;
    private HashSet<string> showAchievements = new HashSet<string>();

    /// <summary>
    /// 도전과제 출력 메서드
    /// 예AchivementManager.Instance.SpawnAchivement("A0002");
    /// </summary>
    /// <param name="soId"></param>
    public void SpawnAchivement(string soId)
    {
        if (showAchievements.Contains(soId))//이미 달성한적 있는 도전과제 출력 x
        {
            return;
        }
        AchievementSO so = ResourceManager.Instance.Load<AchievementSO>($"Data/SO/AchievementSO/{soId}");

        var achivePref = ResourceManager.Instance.Load<AchivementDataContainer>("Prefabs/UI/AchievementItem");

        var pooledAchive = ObjectPoolManager.Instance.GetObject(achivePref, Vector3.zero, Quaternion.identity, 3f);

        pooledAchive.transform.SetParent(UIManager.Instance.CurMainUI.transform, false);


        if (pooledAchive != null)
        {
            achivementDataContainer = pooledAchive.GetComponent<AchivementDataContainer>();
            achivementDataContainer.SetData(so);

            showAchievements.Add(soId);
        }

    }
}
