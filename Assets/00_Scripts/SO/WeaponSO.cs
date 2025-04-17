using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "WeaponSO", menuName = "SO/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    public string ID;
    public string Name;
    public string Description;
    public float ShootRecoil;
    public float DMG;
    public float ReloadTime;
    public int MaxAmmo;
    public int Cost;
}

[System.Serializable]
public class WeaponData
{
    public string ID;
    public string Name;
    public string Description;
    public float ShootRecoil;
    public float DMG;
    public float ReloadTime;
    public int MaxAmmo;
    public int Cost;
}
