using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WeaponStatHandler : MonoBehaviour
{
    [Header("Weapon Data (SO)")]
    public WeaponSO weaponData;

    [Header("Weapon State")]
    public bool isReloading = false; //장전 중인지
    public bool isADS = false; //정조준 중인지

    [Header("Transforms")]
    public Transform barrelLocation; //총구
    public Transform casingExitLocation;// 탄피 배출구

    // 외부에서 할당 받을 참조들
    public Transform handransform; //손떨림을 위한 총을 쥔 손 transform
    public Transform camRoot; //반동을 위한 카메라 부모 transform
    public Camera playerCam; //플레이어 카매라    
    public FpsCamera fpsCamera;
    public GameObject playerObject;

    [Header("Prefabs")]
    private List<ItemSO> equippedParts = new List<ItemSO>();
    public GameObject casingPrefab; //탄피
    public GameObject muzzleFlashPrefab; //총구 화염
    public GameObject bulletImpactPrefab; //탄흔

    [Header("PartsPrefabs")]
    public GameObject redDot;
    public GameObject laserPointer;
    public GameObject compensator;

    [Header("Settings")]
    [Tooltip("총구 각도 퍼짐")]
    [Range(0f, 20f)]
    public float spreadAngle = 10.5f;
    [Tooltip("탄피 삭제 시간")]
    public float destroyTimer = 2f;
    [Tooltip("탄피 배출 파워")]
    public float ejectPower = 150f;
    [Tooltip("발사 쿨타임")]
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

    public float itemRecoil = 0;


    [HideInInspector] public Vector3 camRootOriginPos;
    [HideInInspector] public Quaternion initialLocalRotation;
    [HideInInspector] public float lastFireTime = 0f;

    // 공유 변수 세팅
    public void SetSharedReferences(Transform hand, Transform camRoot, Camera cam, FpsCamera fps, GameObject player)
    {
        this.handransform = hand;
        this.camRoot = camRoot;
        this.playerCam = cam;
        this.fpsCamera = fps;
        this.playerObject = player;
    }

    // 필요하면 getter 추가
    public Transform GetHandTransform() => handransform;
    public Transform GetCamRoot() => camRoot;
    public Camera GetPlayerCamera() => playerCam;
    public FpsCamera GetFpsCamera() => fpsCamera;
    public GameObject GetPlayerObject() => playerObject;

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
        if (!equippedParts.Contains(item))
        {
            equippedParts.Remove(item);
            RemoveItemStats(item);

        }
    }
    private void ApplyItemStats(ItemSO item)
    {
        // weaponData.RCL += item.RCL;
        // weaponData.HDL += item.HDL;
        // weaponData.STP += item.STP;
        // weaponData.SPD += item.SPD;
        weaponData.DMG += item.DMG;
        weaponData.MaxAmmo += item.MaxAmmo;
        itemRecoil += item.ShootRecoil;
    }
    private void RemoveItemStats(ItemSO item)
    {
        // weaponData.RCL += item.RCL;
        // weaponData.HDL += item.HDL;
        // weaponData.STP += item.STP;
        // weaponData.SPD += item.SPD;
        weaponData.DMG -= item.DMG;
        weaponData.MaxAmmo -= item.MaxAmmo;
        itemRecoil -= item.ShootRecoil;
    }
    public void ToggleAttachment(GameObject attachment)
    {
        if (attachment == null) return;

        bool isActive = attachment.activeSelf;
        attachment.SetActive(!isActive);

        ItemReference itemRef = attachment.GetComponent<ItemReference>();
        if (itemRef == null || itemRef.itemData == null) return;

        if (!isActive)
        {
            ApplyItemStats(itemRef.itemData); // 켤 때 스탯 적용
        }
        else
        {
            RemoveItemStats(itemRef.itemData); // 끌 때 스탯 제거
        }
    }
}
