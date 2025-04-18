using System.Linq;
using UnityEngine;

public class RewardSystem
{
    private readonly ItemSO[] itemRewardPool = ResourceManager.Instance.LoadAll<ItemSO>("Data/SO/ItemSO");    // 모든 아이템 SO

    //TODO: 조건 더 추가해야 함
    /// <summary>
    /// Item Pool에서 중복 없이 랜덤으로 3개 반환
    /// </summary>
    public ItemSO[] GetRandomItemReward()
    {
        return itemRewardPool.OrderBy(o => Random.value).Take(3).ToArray();
    }
}
