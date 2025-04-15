using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "Weapon", menuName = "SO/WeaponDatas")]

public class WeaponDatas : ScriptableObject
{
    public string ID;
    public string Name;
    public string Description;
    public float ShootRecoil;
    public int DMG;
    public float ReloadTime;
    public int MaxAmmo;
    public int Cost;

    [Header("Sound")]
    public AudioClip fireSound;
    public AudioClip reloadSound;
    public AudioClip emptySound;
    public AudioClip shellSound;

    [TextArea]
    public string comment;

    [System.Serializable]
    public class PistolDataWrapper
    {
        public WeaponDatas[] Pistols;
    }
}
