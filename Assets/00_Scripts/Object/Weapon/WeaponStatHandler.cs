using System.Collections;
using UnityEngine;

public class WeaponStatHandler : MonoBehaviour
{
    public WeaponData weaponData;
    public Animator gunAnimator;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;
    public GameObject bulletImpactPrefab;
    public GameObject playerCam;
    public bool isReloading;

    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [Tooltip("Specify time to destroy the casing object")]
    [SerializeField] private float destroyTimer = 2f;
    [Tooltip("Casing Ejection Speed")]
    [SerializeField] private float ejectPower = 150f;

    private float fireCooldown = 0.7f;
    private float lastFireTime = 0f;

    [SerializeField] private Transform gunTransform;
    private Quaternion initialLocalRotation;
    void Start()
    {
        LoadWeaponData();

        if (barrelLocation == null)
        {
            barrelLocation = transform;
        }

        if (gunAnimator == null)
        {
            gunAnimator = GetComponentInChildren<Animator>();
        }

        initialLocalRotation = gunTransform.localRotation;
    }

    void Update()
    {
        WeaponShake();

        if (Input.GetButtonDown("Fire1") && Time.time - lastFireTime >= fireCooldown)
        {
            FireWeapon();
        }
        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            ReloadWeapon();
        }
    }
    #region 발사
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
                GunRecoil();
                SoundManager.Instance.PlaySFX("M1911Fire");

                weaponData.currentAmmo--;
                lastFireTime = Time.time;
            }else
            {
                SoundManager.Instance.PlaySFX("EmptyTrigger");
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
                impact.transform.SetParent(hit.collider.transform);
                Destroy(impact, 2f);
            }
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Target"))
            {
                Target target = hit.collider.GetComponentInParent<Target>();
                if (target != null)
                {
                    target.TakeDamage(weaponData.damage, hit.collider);
                }
            }
        }
        StartCoroutine(CameraShake(weaponData.cameraShakeRate * 0.005f));
    }

    private IEnumerator CameraShake(float intensity)
    {
        Vector3 originalPos = playerCam.transform.localPosition;

        float duration = 0.25f;
        float timer = 0f;

        while (timer < duration)
        {
            float damper = 1f - (timer / duration);

            float x = Random.Range(-1f, 1f) * intensity * damper;
            float y = Random.Range(-1f, 1f) * intensity * damper;

            playerCam.transform.localPosition = originalPos + new Vector3(x, y, 0f);

            timer += Time.deltaTime;
            yield return null;
        }

        playerCam.transform.localPosition = originalPos;
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


    // 머즐 플래시 처리
    void MuzzleFlash()
    {
        //ToDO: 오브젝트 풀링으로 관리
        if (muzzleFlashPrefab)
        {
            GameObject tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);
            tempFlash.transform.SetParent(barrelLocation.transform);
            Destroy(tempFlash, destroyTimer);
        }
    }
    #endregion

    #region 총기 흔들림
    //손떨림 
    //ToDo : 정조준시 흔들리게 바꿔야함
    private void WeaponShake()
    {
        if (weaponData == null || gunTransform == null)
        {
            return;
        }

        float accuracy = Mathf.Clamp01((99f - weaponData.accuracy) / 98f);
        float shakeAmount = accuracy * 7.5f;
        float shakeSpeed = 0.7f;

        float rotX = (Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) - 0.5f) * shakeAmount * 1f;
        float rotY = (Mathf.PerlinNoise(0f, Time.time * shakeSpeed) - 0.5f) * shakeAmount * 3f;
        float rotZ = (Mathf.PerlinNoise(Time.time * shakeSpeed, Time.time * shakeSpeed) - 0.5f) * shakeAmount;

        Quaternion shakeRotation = Quaternion.Euler(rotX, rotY, rotZ);

        gunTransform.localRotation = initialLocalRotation * shakeRotation;
    }

    //반동
    private void GunRecoil()
    {
        playerCam.transform.localRotation *= Quaternion.Euler(-weaponData.shootRecoil * 0.01f, 0f, 0f);
    }
    #endregion

    // WeaponData 로드
    void LoadWeaponData()
    {
        TextAsset jsonData = Resources.Load<TextAsset>("Data/JSON/PistolData");

        if (jsonData != null)
        {
            weaponData = JsonUtility.FromJson<WeaponData>(jsonData.text);
        }
    }

    #region 재장전
    void ReloadWeapon()
    {
        if (weaponData != null && weaponData.currentAmmo != weaponData.maxAmmo)
        {
            isReloading = true;
            weaponData.currentAmmo = 0;
            gunAnimator.SetTrigger("Reload");
            SoundManager.Instance.PlaySFX("Reload");
            StartCoroutine(WaitForEndOfReload());

        }
    }

    private IEnumerator WaitForEndOfReload()
    {
        yield return new WaitForSeconds(weaponData.reloadTime);
        gunAnimator.SetBool("OutOfAmmo", false);
        weaponData.currentAmmo = weaponData.maxAmmo;
        isReloading = false;

    }
    #endregion
}
