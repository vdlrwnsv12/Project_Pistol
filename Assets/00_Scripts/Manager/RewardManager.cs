using System.Linq;
using UnityEngine;

public class RewardManager
{
    private readonly ItemData[] itemRewardPool = ResourceManager.Instance.LoadAll<ItemData>("Data/SO/Item");

    /// <summary>
    /// Item Pool에서 중복 없이 랜덤으로 3개 반환
    /// </summary>
    /// <returns></returns>
    public ItemData[] GetRandomItemReward()
    {
        return itemRewardPool.OrderBy(o => Random.value).Take(3).ToArray();
    }
}
