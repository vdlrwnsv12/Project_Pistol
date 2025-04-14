using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Weapon/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public int ID;
    public string weaponName;
    public int damage;
    public float shootRecoil;
    public float reloadTime;
    public int maxAmmo;
    public int currentAmmo;
    //public float accuracy;
    public bool isSilenced;
    public float cameraShakeRate;

    [Header("Sound")]
    public AudioClip fireSound;
    public AudioClip reloadSound;
    public AudioClip emptySound;
    public AudioClip shellSound;

    [TextArea]
    public string comment;
}

[System.Serializable]
public class PistolDataWrapper
{
    public WeaponData[] Pistols;
}

