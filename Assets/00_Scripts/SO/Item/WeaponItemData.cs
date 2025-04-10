using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "SO/ItemData/WeaponItem")]
public class WeaponItemData : ItemData
{
    public WeaponStat stat;
}

[Serializable]
public struct WeaponStat
{
    public float dmgValue;
    public float maxAmmoValue;
    public bool isScope;
    public bool isLaserSite;
}
