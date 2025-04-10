using UnityEngine;

public class WeaponStatHandler : MonoBehaviour
{
    [Header("Weapon Data")]
    public int weaponID = 0;
    public WeaponData weaponData;
    public bool isReloading = false;
    public bool isADS = false;

    [Header("Transforms")]
    public Transform barrelLocation;
    public Transform casingExitLocation;
    public Transform gunTransform;
    public Transform camRoot;

    [Header("Camera")]
    public Camera playerCam;
    public FpsCamera fpsCamera;

    [Header("Player")]
    public GameObject playerObject;

    [Header("Prefabs")]
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;
    public GameObject bulletImpactPrefab;

    [Header("Settings")]
    [Tooltip("총구 각도 퍼짐")]
    public float spreadAngle = 5f;
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

    [HideInInspector] public Vector3 camRootOriginPos;
    [HideInInspector] public Quaternion initialLocalRotation;
    [HideInInspector] public float lastFireTime = 0f;

    void Awake()
    {
        LoadWeaponDataByID(weaponID);

        if (weaponData == null)
        {
            Debug.LogWarning($"Weapon data with ID {weaponID} not found.");
        }
    }

    void LoadWeaponDataByID(int id)
    {
        TextAsset jsonData = Resources.Load<TextAsset>("Data/JSON/PistolData");

        if (jsonData != null)
        {
            PistolDataWrapper wrapper = JsonUtility.FromJson<PistolDataWrapper>(jsonData.text);

            foreach (WeaponData data in wrapper.Pistols)
            {
                if (data.ID == id)
                {
                    weaponData = data;
                    break;
                }
            }
        }
    }
}
