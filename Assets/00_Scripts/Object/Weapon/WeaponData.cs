using UnityEngine;

[System.Serializable]
public class WeaponData
{
    public int ID;
    public string weaponName;
    public int damage;
    public float shootRecoil;
    public float reloadTime;
    public int maxAmmo;
    public int currentAmmo;
    public float accuracy;
    public bool isSilenced;
    public float cameraShakeRate;

    [TextArea]
    public string comment;
}

[System.Serializable]
public class PistolDataWrapper
{
    public WeaponData[] Pistols;
}

