using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchivementManager : SingletonBehaviour<AchivementManager>
{
    private AchivementDataContainer achivementDataContainer;

    /// <summary>
    /// 도전과제 출력 메서드
    /// 예AchivementManager.Instance.SpawnAchivement("A0002");
    /// </summary>
    /// <param name="soId"></param>
    public void SpawnAchivement(string soId)
    {
        AchievementSO so = ResourceManager.Instance.Load<AchievementSO>($"Data/SO/AchievementSO/{soId}");

        var achivePref = ResourceManager.Instance.Load<AchivementDataContainer>("Prefabs/UI/AchivementItem");

        var pooledAchive = ObjectPoolManager.Instance.GetObject(achivePref, Vector3.zero, Quaternion.identity, 3f);

        pooledAchive.transform.SetParent(UIManager.Instance.CurMainUI.transform, false);


        if (achivePref != null)
        {
            achivementDataContainer = pooledAchive.GetComponent<AchivementDataContainer>();
        }

        achivementDataContainer.SetData(so);
    }
}
