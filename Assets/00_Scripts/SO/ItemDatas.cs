using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "Item", menuName = "SO/ItemDatas")]

public class ItemDatas : ScriptableObject
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
public float ShootRecoil;
public int MaxAmmo;
public int WeaponParts;
public float AppearanceRate;
}
