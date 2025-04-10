using System;
using DataDeclaration;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "SO/ItemData")]
public class ItemData : ScriptableObject
{
    public string id;
    public string itemName;
    public string description;
    public ItemStat stat;
    public ItemApplyTargetType targetType;
    public float cost;
}

[Serializable]
public struct ItemStat
{
    public float rclValue;
    public float hdlValue;
    public float stpValue;
    public float spdValue;
    
    public float dmgValue;

    public int maxBulletValue;
}

public enum ItemApplyTargetType
{
    Player,
    Weapon
}
