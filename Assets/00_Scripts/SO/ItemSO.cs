using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "ItemSO", menuName = "SO/ItemSO")]
public class ItemSO : ScriptableObject
{
    public string ID;
    public string Name;
    public string Description;
    public int ApplicationTarget;
    public float cost;
    public float RCL;
    public float HDL;
    public float STP;
    public float SPD;
    public float DMG;
    public string ShootRecoil;
    public int MaxAmmo;
    public int WeaponParts;
    public string AppearanceRate;
}

[System.Serializable]
public class ItemData
{
    public string ID;
    public string Name;
    public string Description;
    public int ApplicationTarget;
    public float cost;
    public float RCL;
    public float HDL;
    public float STP;
    public float SPD;
    public float DMG;
    public string ShootRecoil;
    public int MaxAmmo;
    public int WeaponParts;
    public string AppearanceRate;
}
