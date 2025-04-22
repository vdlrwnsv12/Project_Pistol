using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Delete
{
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


    public void BindToWeapon(WeaponFireController fireController)
    {
        onAmmoChanged += UpdateBulletText;
    }

/// <summary>
/// 현재 총알 상태
/// </summary>
/// <param name="currentAmmo">MaxAmmo만 바꿀떈 여기에 -1넣으셈</param>
/// <param name="MaxAmmo"></param>
    private void UpdateBulletText(int currentAmmo, int MaxAmmo)
    {
        if (currentAmmo < 0)
        {
            string[] split = bulletStatText.text.Split('/');
            string oldAmmo = split[0].Trim();
            bulletStatText.text = $"{oldAmmo} / {MaxAmmo}";
        }else
        {
            bulletStatText.text = $"{currentAmmo} / {MaxAmmo}";
        }
    }
}
}

