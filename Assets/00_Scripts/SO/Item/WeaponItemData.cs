using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "SO/ItemData/WeaponItem")]
public class WeaponItemData : ItemData
{
    [field: SerializeField] public ItemWeaponStat Stat { get; private set; }
}

[Serializable]
public struct ItemWeaponStat
{
    public float dmgValue;
    public float maxAmmoValue;
    public bool isScope;
    public bool isLaserSite;
}
