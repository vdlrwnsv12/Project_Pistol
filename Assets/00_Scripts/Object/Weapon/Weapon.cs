using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponController), typeof(Animator))]
public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponSO data;

    // [SerializeField] private List<ItemSO> equippedParts = new List<ItemSO>();

    private int equippedPartsMask = 0;

    public int CurAmmo => Controller.CurAmmo;
    public int MaxAmmo => Stat.MaxAmmo;

    public WeaponSO Data => data;
    public WeaponStatHandler Stat { get; private set; }
    public WeaponController Controller { get; private set; }

    public Animator Anim { get; private set; }

    [SerializeField] private GameObject i2003;
    [SerializeField] private GameObject i2004;
    [SerializeField] private GameObject i2005;
    [SerializeField] private GameObject i2006;

    private void Update()//테스트
    {
        // 1번 키를 누르면 첫 번째 아이템을 토글
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ToggleAttachment(i2003);  // Item1은 실제 아이템 객체로 바꿔주세요
        }
        // 2번 키를 누르면 두 번째 아이템을 토글
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ToggleAttachment(i2004);  // Item2는 실제 아이템 객체로 바꿔주세요
        }
        // 3번 키를 누르면 세 번째 아이템을 토글
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ToggleAttachment(i2005);  // Item3은 실제 아이템 객체로 바꿔주세요
        }
        // 4번 키를 누르면 네 번째 아이템을 토글
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ToggleAttachment(i2006);  // Item4는 실제 아이템 객체로 바꿔주세요
        }
    }

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
        int group = item.WeaponParts;
        bool willBeActive = !attachment.activeSelf;

        //같은 그룹 파츠 하나만 활성화
        if (willBeActive)
        {
            DisableSameGroupParts(attachment, group);
            EnableParts(attachment, item, group);
        }
    }

    private void DisableSameGroupParts(GameObject current, int group)
    {
        foreach (Transform child in current.transform.parent)
        {
            GameObject sibling = child.gameObject;
            if (sibling == current || !sibling.activeSelf) continue;

            ItemInformation siblingInfo = sibling.GetComponent<ItemInformation>();
            if (siblingInfo == null || siblingInfo.itemData == null) continue;

            if (siblingInfo.itemData.WeaponParts == group)
            {
                sibling.SetActive(false);
                Stat.RemoveStat(siblingInfo.itemData.DMG, -siblingInfo.itemData.ShootRecoil, siblingInfo.itemData.MaxAmmo, group);
            }
        }
    }



    private void EnableParts(GameObject attachment, ItemSO item, int group)
    {
        attachment.SetActive(true);
        Stat.RemoveStat(item.DMG, -item.ShootRecoil, item.MaxAmmo, group);
        equippedPartsMask |= (1 << group);

        if(group == 1) //활성화 파츠가 optic이라면 시점 상승 근데...이게 맞나? 이렇게 하면 안될거같은데
        {
            Transform adsCamTransform = GameObject.Find("AdsCamTransform")?.transform; 
            if(adsCamTransform != null)
            {
                Vector3 pos = adsCamTransform.localPosition;
                pos.z = 0.0018f;
                adsCamTransform.localPosition = pos;
            }
        }
    }

}