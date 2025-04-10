using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "SO/ItemData/PlayerItem")]
public class PlayerItemData : ItemData
{
    public ItemStat stat;
}

[Serializable]
public struct ItemStat
{
    public float rclValue;
    public float hdlValue;
    public float stpValue;
    public float spdValue;
}