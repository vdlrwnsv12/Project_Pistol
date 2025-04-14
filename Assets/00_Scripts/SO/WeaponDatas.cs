using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "Weapon", menuName = "SO/WeaponDatas")]

public class WeaponDatas : ScriptableObject
{
public string ID;
public string Name;
public string Description;
public float ShootRecoil;
public float DMG;
public float ReloadTime;
public int MaxAmmo;
}
