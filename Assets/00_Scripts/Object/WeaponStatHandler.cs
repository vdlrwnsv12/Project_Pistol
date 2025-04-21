namespace test
{
    public class WeaponStatHandler
    {
        private int weaponParts;
        
        public float Damage { get; private set; }

        public int MaxAmmo { get; private set; }

        public float FinalRecoil { get; private set; }

        public float ReloadTime { get; private set; }

        public WeaponStatHandler(WeaponSO data)
        {
            Damage = data.DMG;
            MaxAmmo = data.MaxAmmo;
            FinalRecoil = data.ShootRecoil;
            ReloadTime = data.ReloadTime;

            weaponParts = 0;
        }

        public void ChangeStat(float dmg, float recoil, int ammo, int partsValue)
        {
            Damage += dmg;
            FinalRecoil += recoil;
            MaxAmmo += ammo;
            weaponParts <<= partsValue;
        }
    }
}
