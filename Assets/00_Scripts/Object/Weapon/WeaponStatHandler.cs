using UnityEngine;

public class WeaponStatHandler : MonoBehaviour
{
    public WeaponData weaponData;
    public Animator gunAnimator;

    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [Tooltip("Specify time to destroy the casing object")]
    [SerializeField] private float destroyTimer = 2f;
    [Tooltip("Casing Ejection Speed")]
    [SerializeField] private float ejectPower = 150f;

    private float fireCooldown = 0.5f;
    private float lastFireTime = 0f;

    void Start()
    {
        LoadWeaponData();

        if (barrelLocation == null)
            barrelLocation = transform;

        if (gunAnimator == null)
            gunAnimator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if(weaponData.currentAmmo == 1 && Input.GetButtonDown("Fire1"))
        {
            weaponData.currentAmmo--;
            gunAnimator.SetBool("OutOfAmmo", true);
            CasingRelease();
            MuzzleFlash();
            return;
        }
        if (Input.GetButtonDown("Fire1") && Time.time - lastFireTime >= fireCooldown)
        {
            FireWeapon();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadWeapon();
        }
    }

    public void FireWeapon()
    {
        if (weaponData != null)
        {
            if (weaponData.currentAmmo > 0)
            {
                if (gunAnimator != null)
                {
                    gunAnimator.SetTrigger("Fire");
                }

                // 레이 발사
                ShootRay();
                CasingRelease();
                MuzzleFlash();

                weaponData.currentAmmo--;
                lastFireTime = Time.time;
                
            }
            
        }
    }

    public void ShootRay()
    {
        Ray ray = new Ray(barrelLocation.position, barrelLocation.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Hit: " + hit.collider.name);

        }
    }

    // 탄피 배출 처리
    void CasingRelease()
    {
        if (casingExitLocation && casingPrefab)
        {
            GameObject tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation);
            Rigidbody casingRb = tempCasing.GetComponent<Rigidbody>();

            if (casingRb != null)
            {
                casingRb.AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower),
                                          (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
                casingRb.AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);
            }

            Destroy(tempCasing, destroyTimer);
        }
    }

    // 뮤즐 플래시 처리
    void MuzzleFlash()
    {
        if (muzzleFlashPrefab)
        {
            GameObject tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);
            Destroy(tempFlash, destroyTimer);
        }
    }

    // WeaponData 로드
    void LoadWeaponData()
    {
        TextAsset jsonData = Resources.Load<TextAsset>("Data/JSON/PistolData");

        if (jsonData != null)
        {
            weaponData = JsonUtility.FromJson<WeaponData>(jsonData.text);
        }
    }

   
    void ReloadWeapon()
    {
        if (weaponData != null)
        {
            weaponData.currentAmmo = weaponData.maxAmmo;

            // 재장전 애니메이션 트리거
            if (gunAnimator != null)
            {
                gunAnimator.SetTrigger("Reload");
            }
            gunAnimator.SetBool("OutOfAmmo", false);
        }
    }
}
