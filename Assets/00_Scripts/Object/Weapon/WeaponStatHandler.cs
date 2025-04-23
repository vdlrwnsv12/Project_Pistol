public class WeaponStatHandler
{
    private int weaponParts;

    public float Damage { get; private set; }

    public int MaxAmmo { get; private set; }

    public float Recoil { get; private set; }

    public float ReloadTime { get; private set; }

    public WeaponStatHandler(WeaponSO data)
    {
        Damage = data.DMG;
        MaxAmmo = data.MaxAmmo;
        Recoil = data.ShootRecoil;
        ReloadTime = data.ReloadTime;

        weaponParts = 0;
    }

    public void ChangeStat(float dmg, float recoil, int ammo, int partsValue)
    {
        Damage += dmg;
        Recoil += recoil;
        MaxAmmo += ammo;
        weaponParts = 1 << partsValue;
    }
}