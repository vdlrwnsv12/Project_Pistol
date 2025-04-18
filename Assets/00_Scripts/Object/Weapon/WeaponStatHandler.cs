using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponStatHandler : MonoBehaviour
{
    [Header("Weapon Data (SO)")]
    public WeaponSO weaponData;

    [Header("Weapon State")]
    public bool isReloading = false;
    public bool isADS = false;

    [Header("Weapon Stat")]
    public string ID;
    public string Name;
    public string Description;
    public float ShootRecoil;
    public float DMG;
    public float ReloadTime;
    public int MaxAmmo;
    public int Cost;

    [Header("Transforms")]
    public Transform barrelLocation;
    public Transform casingExitLocation;
    public Transform handransform;
    public Transform camRoot;
    public Camera playerCam;
    public FpsCamera fpsCamera;
    public GameObject playerObject;
    public Text bulletStatText;

    [Header("Prefabs")]
    private List<ItemSO> equippedParts = new List<ItemSO>();
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;
    public GameObject bulletImpactPrefab;

    [Header("PartsPrefabs")]
    public GameObject redDot;
    public GameObject holographic;
    public GameObject laserPointer;
    public GameObject compensator;

    [Header("Settings")]
    [Range(0f, 20f)]
    public float spreadAngle = 10.5f;
    public float destroyTimer = 2f;
    public float ejectPower = 150f;
    public float fireCooldown = 0.7f;

    [Header("ADS Settings")]
    public Vector3 adsPosition = new Vector3(0.062f, -0.007f, 0f);
    public float camMoveSpeed = 10f;

    [Header("Animator")]
    public Animator gunAnimator;

    [Header("Sound Container")]
    public AudioClip fireSound;
    public AudioClip reloadSound;
    public AudioClip emptySound;

    [Header("Item Stat")]
    public float itemRecoil = 0;

    [HideInInspector] public Vector3 camRootOriginPos;
    [HideInInspector] public Quaternion initialLocalRotation;
    [HideInInspector] public float lastFireTime = 0f;
    public Action<int, int> onAmmoChanged;

    public void WeaponDataFromSO() // ScriptableObject로부터 스탯 불러오기
    {
        if (weaponData == null)
        {
            Debug.Log("WeaponSO없음");
        }

        ID = weaponData.ID;
        Description = weaponData.Description;
        ShootRecoil = weaponData.ShootRecoil;
        DMG = weaponData.DMG;
        ReloadTime = weaponData.ReloadTime;
        MaxAmmo = weaponData.MaxAmmo;
        Cost = weaponData.Cost;
    }

    // PlayerEquipment에서 참조 넘겨받음
    public void SetSharedReferences(Transform hand, Transform camRoot, Camera cam, FpsCamera fps, GameObject player, Text bulletText)
    {
        this.handransform = hand;
        this.camRoot = camRoot;
        this.playerCam = cam;
        this.fpsCamera = fps;
        this.playerObject = player;
        this.bulletStatText = bulletText;
    }

    public Transform GetHandTransform() => handransform;
    public Transform GetCamRoot() => camRoot;
    public Camera GetPlayerCamera() => playerCam;
    public FpsCamera GetFpsCamera() => fpsCamera;
    public GameObject GetPlayerObject() => playerObject;

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

    private void ApplyItemStats(ItemSO item) // 아이템 효과 반영
    {
        DMG += item.DMG;
        MaxAmmo += item.MaxAmmo;
        //ShootRecoil *= 1f - (item.ShootRecoil * 0.01f);
    }

    private void RemoveItemStats(ItemSO item)
    {
        DMG -= item.DMG;
        MaxAmmo -= item.MaxAmmo;
        //ShootRecoil /= 1f - (item.ShootRecoil * 0.01f);
    }
    /// <summary>
    /// 조준경 아닌 파츠 사용
    /// </summary>
    /// <param name="파츠이름F"></param>
    public void ToggleAttachment(GameObject attachment) // 파츠 토글
    {
        if (attachment == null) return;

        bool isActive = attachment.activeSelf;
        ItemReference itemRef = attachment.GetComponent<ItemReference>();

        if (!isActive)
        {
            EquipItem(itemRef.itemData);
        }
        else
        {
            UnEquipItem(itemRef.itemData);
        }

        attachment.SetActive(!isActive);

        if (itemRef == null || itemRef.itemData == null) return;
    }

    /// <summary>
    /// 조준경 파츠 사용
    /// </summary>
    /// <param name="조준경 이름"></param>
    public void ToggleOpticAttachment(GameObject selectedOptic) //조준경 토글
    {
        if (selectedOptic == null) return;

        // optics 그룹
        List<GameObject> optics = new List<GameObject> { redDot, holographic };

        foreach (var optic in optics)
        {
            if (optic == null) continue;
            if (optic == selectedOptic)
            {
                bool willBeActive = !optic.activeSelf;
                optic.SetActive(willBeActive);

                ItemReference itemRef = optic.GetComponent<ItemReference>();
                if (itemRef != null && itemRef.itemData != null)
                {
                    if (willBeActive)
                        EquipItem(itemRef.itemData);
                    else
                        UnEquipItem(itemRef.itemData);
                }
            }
            else
            {
                if (optic.activeSelf)
                {
                    optic.SetActive(false);
                    ItemReference itemRef = optic.GetComponent<ItemReference>();
                    if (itemRef != null && itemRef.itemData != null)
                        UnEquipItem(itemRef.itemData);
                }
            }
        }
    }
    public void BindToWeapon(WeaponFireController fireController)
    {
        onAmmoChanged += UpdateBulletText;
    }

    private void UpdateBulletText(int currentAmmo, int MaxAmmo)
    {
        bulletStatText.text = $"{currentAmmo} / {MaxAmmo}";
    }
}
