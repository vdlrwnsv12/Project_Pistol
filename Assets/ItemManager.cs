public class ItemManager
{
    public static ItemData Data(ItemSO so)
    {
        return new ItemData
        {
            ID = so.ID,
            Name = so.Name,
            Description = so.Description,
            ApplicationTarget = so.ApplicationTarget,
            cost = so.cost,
            RCL = so.RCL,
            HDL = so.HDL,
            STP = so.STP,
            SPD = so.SPD,
            DMG = so.DMG,
            ShootRecoil = so.ShootRecoil,
            MaxAmmo = so.MaxAmmo,
            WeaponParts = so.WeaponParts,
            AppearanceRate = so.AppearanceRate
        };
    }
}
