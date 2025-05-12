using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponController), typeof(Animator))]
public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponSO data;

    [SerializeField] private List<ItemSO> equippedParts = new List<ItemSO>();

    private int equippedPartsMask = 0;

    public int CurAmmo => Controller.CurAmmo;
    public int MaxAmmo => Stat.MaxAmmo;

    public WeaponSO Data => data;
    public WeaponStatHandler Stat { get; private set; }
    public WeaponController Controller { get; private set; }

    public Animator Anim { get; private set; }

    private void Awake()
    {
        if (data == null)
        {
            data = ResourceManager.Instance.Load<WeaponSO>(
                $"Data/SO/CharacterSO/{gameObject.name.Replace("(Clone)", "").Trim()}");
        }

        Stat = new WeaponStatHandler(data);
        Controller = GetComponent<WeaponController>();

        Anim = GetComponent<Animator>();
    }

    /// <summary>
    /// 조준경 아닌 파츠 사용
    /// </summary>
    /// <param name="attachment">파츠이름</param>
    public void ToggleAttachment(GameObject attachment) // 파츠 토글
    {
        if (attachment == null) return;

        ItemInformation itemInfo = attachment.GetComponent<ItemInformation>();
        if (itemInfo == null || itemInfo.itemData == null) return;

        ItemSO item = itemInfo.itemData;
        int partGroup = item.WeaponParts;
        bool willBeActive = !attachment.activeSelf;

        if(willBeActive)
        {
            if((equippedPartsMask & (1 << partGroup)) != 0)
            {
                foreach(Transform child in attachment.transform.parent)
                {
                    GameObject sibling = child.gameObject;
                    if(sibling == attachment || !sibling.activeSelf) continue;
                    
                    ItemInformation siblingInfo = sibling.GetComponent<ItemInformation>();
                    if(sibling == null || siblingInfo.itemData == null) continue;

                    if (siblingInfo.itemData.WeaponParts == partGroup)
                    {
                        sibling.SetActive(false);
                        Stat.RemoveStat(siblingInfo.itemData.DMG, siblingInfo.itemData.ShootRecoil, siblingInfo.itemData.MaxAmmo, partGroup);
                    }
                }
            }
            attachment.SetActive(true);
            Stat.ChangeStat(item.DMG, item.ShootRecoil, item.MaxAmmo, partGroup);
            
            equippedPartsMask |= (1 << partGroup);
        }
        else
        {
            attachment.SetActive(false);
            Stat.RemoveStat(item.DMG, item.ShootRecoil, item.MaxAmmo, partGroup);

            equippedPartsMask &= ~(1 << partGroup);
        }
    }

}