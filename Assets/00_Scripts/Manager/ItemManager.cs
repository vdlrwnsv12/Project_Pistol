using System.Collections.Generic;
using UnityEngine;

public class ItemManager : SingletonBehaviour<ItemManager>
{
    public WeaponStatHandler weaponStatHandler;
    public PlayerStatHandler playerStatHandler;

    [SerializeField]private List<ItemSO> equippedParts = new List<ItemSO>();
   
    /// <summary>
    /// 조준경 아닌 파츠 사용
    /// </summary>
    /// <param name="attachment">파츠이름</param>
    public void ToggleAttachment(GameObject attachment) // 파츠 토글
    {
        if (attachment == null) return;

        ItemReference itemRef = attachment.GetComponent<ItemReference>();
        if (itemRef == null || itemRef.itemData == null) return;

        ItemSO item = itemRef.itemData;
        int partGroup = item.WeaponParts;
        bool willBeActive = !attachment.activeSelf;

        if(partGroup > 0 && willBeActive)
        {
            foreach (Transform child in attachment.transform.parent)
            {
                GameObject sibling = child.gameObject;
                if(sibling == attachment || !sibling.activeSelf) continue;

                ItemReference siblingRef = sibling.GetComponent<ItemReference>();
                if(siblingRef == null || siblingRef.itemData == null) continue;

                if(siblingRef.itemData.WeaponParts == partGroup)
                {
                    sibling.SetActive(false);
                    UnEquipItem(siblingRef.itemData);
                }
            }
        }
        attachment.SetActive(willBeActive);

        if(willBeActive)
        {
            EquipItem(item);
        }else
        {
            UnEquipItem(item);
        }
    }

    // 조준경/부착물 교체용 파츠 장착
    public void EquipItem(ItemSO item)
    {
        if (!equippedParts.Contains(item))
        {
            equippedParts.Add(item);
            ApplyItemStats(item);
        }
    }

    public void UnEquipItem(ItemSO item)
    {
        equippedParts.Remove(item);
        RemoveItemStats(item);
    }

     public void ApplyItemStats(ItemSO item) // 아이템 효과 반영
    {
        weaponStatHandler.DMG += item.DMG;

        if(item.MaxAmmo != 0)
        {
             weaponStatHandler.MaxAmmo += item.MaxAmmo;
             //weaponStatHandler.onAmmoChanged?.Invoke(-1, weaponStatHandler.MaxAmmo);//UI
        }
        
        weaponStatHandler.ShootRecoil *= 1f - (item.ShootRecoil * 0.01f);

        playerStatHandler.IncreaseStat(item.RCL, item.HDL, item.STP, item.SPD);

    }

    private void RemoveItemStats(ItemSO item)
    {
        weaponStatHandler.DMG -= item.DMG;
        weaponStatHandler.MaxAmmo -= item.MaxAmmo;
        weaponStatHandler.ShootRecoil /= 1f - (item.ShootRecoil * 0.01f);
    }
}
