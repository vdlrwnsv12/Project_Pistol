using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public class WeaponStatHandler : MonoBehaviour
{
    public WeaponData weaponData;
    public Animator gunAnimator;

    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;
    public GameObject bulletImpactPrefab;

    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [Tooltip("Specify time to destroy the casing object")]
    [SerializeField] private float destroyTimer = 2f;
    [Tooltip("Casing Ejection Speed")]
    [SerializeField] private float ejectPower = 150f;

    private float fireCooldown = 0.7f;
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
                if (gunAnimator != null && weaponData.currentAmmo != 1)
                {
                    gunAnimator.SetTrigger("Fire");

                }
                if (weaponData.currentAmmo == 1)
                {
                    gunAnimator.SetBool("OutOfAmmo", true);
                }

                // 레이 발사
                ShootRay();
                CasingRelease();
                MuzzleFlash();
                SoundManager.Instance.PlaySFX("M1911Fire");

                weaponData.currentAmmo--;
                lastFireTime = Time.time;

            }

        }
    }

    public void ShootRay()
    {
        Ray ray = new Ray(barrelLocation.position, barrelLocation.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 0f);


        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Hit: " + hit.collider.name);

            if (bulletImpactPrefab)
            {
                //ToDO: 오브젝트 풀링으로 관리
                Quaternion hitRotation = Quaternion.LookRotation(hit.normal);
                GameObject impact = Instantiate(bulletImpactPrefab, hit.point, hitRotation);
                Destroy(impact, 2f);
            }
        }
    }

    // 탄피 배출 처리
    void CasingRelease()
    {
        //ToDO: 오브젝트 풀링으로 관리
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
            SoundManager.Instance.PlaySFX("Shell");

            Destroy(tempCasing, destroyTimer);
        }
    }

    // 뮤즐 플래시 처리
    void MuzzleFlash()
    {
        //ToDO: 오브젝트 풀링으로 관리
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
            weaponData.currentAmmo = 0;
            gunAnimator.SetBool("OutOfAmmo", true);

            SoundManager.Instance.PlaySFX("Reload");
            StartCoroutine(WaitForEndOfReload());

        }
    }
    private IEnumerator WaitForEndOfReload()
    {
        yield return new WaitForSeconds(1.6f);
        gunAnimator.SetBool("OutOfAmmo", false);
        weaponData.currentAmmo = weaponData.maxAmmo;

    }
}
