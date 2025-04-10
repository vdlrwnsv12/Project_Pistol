using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "SO/ItemData/PlayerItem")]
public class PlayerItemData : ItemData
{
    [field: SerializeField]public ItemPlayerStat Stat { get; private set; }
}

[Serializable]
public struct ItemPlayerStat
{
    public float rclValue;
    public float hdlValue;
    public float stpValue;
    public float spdValue;
}