using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponFireController : MonoBehaviour
{
     // === [참조용 컴포넌트] ===
    private WeaponStatHandler weaponStatHandler;
    private Player player;

    // === [탄약] ===
    [SerializeField] public int currentAmmo;

    // === [에임 / 조준 관련] ===
    private Vector3 camRootOriginPos;
    private Vector3 currentCamRootTargetPos;
    private Quaternion currentHandTargetRot;
    private Quaternion initialLocalRotation;
    [SerializeField] private float targetCamY = 0.165f; // 조준 시 카메라 Y 위치
    [SerializeField] private float accuracyAmount; // 플레이어 조준 정확도 (손떨림에 영향)

    // === [반동 관련] ===
    [SerializeField] private float finalRecoil;

    // === [테스트용] ===
    [SerializeField] private bool isLocked = true; // F 키로 마우스 잠금 전환 (테스트용)
    [SerializeField] private List<GameObject> optics; // redDot, holographic 등 조준경 그룹

    // === [사운드 관련] ===
    private AudioClip reloadSound;
    private AudioClip emptyTrigger;
    private AudioClip fireSound;

    // === [이펙트 프리팹] ===
    private GameObject casingPrefab;
    private GameObject muzzleFlashPrefab;
    private GameObject bulletImpactPrefab;

    #region Unity Methods

    public void InitReferences()
    {
        weaponStatHandler = GetComponent<WeaponStatHandler>();
        player = weaponStatHandler.player;

        string nameToSerch = gameObject.name.Replace("(Clone)", "").Trim();

        if (weaponStatHandler.weaponData == null)
        {
            
            weaponStatHandler.weaponData = Resources.Load<WeaponSO>($"Data/SO/WeaponSO/{nameToSerch}");
            if (weaponStatHandler.weaponData == null)
            {
                Debug.Log($"[InitReferences] WeaponData '{nameToSerch}'을(를) 찾을 수 없습니다.");
            }
            else
            {
                Debug.Log($"[InitReferences] WeaponData '{nameToSerch}'자동 할당.");
            }
        }

        weaponStatHandler.WeaponDataFromSO();
        initialLocalRotation = weaponStatHandler.handransform.localRotation;
        camRootOriginPos = weaponStatHandler.camRoot.localPosition;
        currentAmmo = weaponStatHandler.MaxAmmo;
        //weaponStatHandler.BindToWeapon(this);
        //weaponStatHandler.onAmmoChanged(currentAmmo, weaponStatHandler.MaxAmmo);
        optics = new List<GameObject> { weaponStatHandler.redDot, weaponStatHandler.holographic };
        accuracyAmount = player.Data.HDL;
        player.weaponFireController = this;

        fireSound = reloadSound = ResourceManager.Instance.Load<AudioClip>($"Audio/SFX/{nameToSerch}");
        reloadSound = ResourceManager.Instance.Load<AudioClip>("Audio/SFX/Reload");
        emptyTrigger = ResourceManager.Instance.Load<AudioClip>("Audio/SFX/EmptyTrigger");

        casingPrefab = ResourceManager.Instance.Load<GameObject>("Prefabs/Props/Case");
        muzzleFlashPrefab = ResourceManager.Instance.Load<GameObject>("Prefabs/Props/MuzzleFlash");
        bulletImpactPrefab = ResourceManager.Instance.Load<GameObject>("Prefabs/Props/BulletHole");

    }

    void Update()
    {
        if (weaponStatHandler == null)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.F))//테스트용 코드
        {
            if (isLocked)
                UnlockCursor();
            else
                LockCursor();
        }
        #region 레이저 포인터 테스트 삭제해야함
        if (weaponStatHandler.laserPointer.activeSelf == true)
        {
            weaponStatHandler.spreadAngle = 0;
        }
        else
        {
            weaponStatHandler.spreadAngle = 10.5f;
        }
        #endregion
        //HandleADS();
    }

    #endregion

    #region 테스트용 코드
    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isLocked = true;
    }

    void UnlockCursor()
    {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isLocked = false;
    }
    #endregion

    #region ADS

    public void HandleADS()
    {
        if (Input.GetMouseButtonDown(1) && !weaponStatHandler.isReloading)
        {
            weaponStatHandler.isADS = !weaponStatHandler.isADS;
        }
        if (weaponStatHandler.isADS)
        {
            currentCamRootTargetPos = weaponStatHandler.adsPosition;
            currentHandTargetRot = initialLocalRotation;

            // redDot 상태에 따라 타겟 Y 설정
            // targetCamY = (weaponStatHandler.redDot != null && weaponStatHandler.redDot.activeSelf) ? 0.18f : 0.16f;
            bool isOpticActive = optics.Exists(optics => optics.activeSelf);//조준경이 하나라도 켜져 있으면
            targetCamY = isOpticActive ? 0.18f : 0.16f;
        }
        else
        {
            currentCamRootTargetPos = camRootOriginPos;
            currentHandTargetRot = initialLocalRotation;

            // 정조준 해제 시 기본값으로 복구
            targetCamY = 0.16f;
        }

        // FOV 보간
        float targetFOV = weaponStatHandler.isADS ? 40f : 60f;
        weaponStatHandler.playerCam.fieldOfView = Mathf.Lerp(weaponStatHandler.playerCam.fieldOfView, targetFOV, Time.deltaTime * 10f);

        // 위치/회전 보간
        weaponStatHandler.camRoot.localPosition = Vector3.Lerp(weaponStatHandler.camRoot.localPosition, currentCamRootTargetPos, Time.deltaTime * weaponStatHandler.camMoveSpeed);
        weaponStatHandler.handransform.localRotation = Quaternion.Lerp(weaponStatHandler.handransform.localRotation, currentHandTargetRot, Time.deltaTime * 10f);

        //Y 위치만 따로 부드럽게 보간
        Vector3 camLocalPos = weaponStatHandler.playerCam.transform.localPosition;
        camLocalPos.y = Mathf.Lerp(camLocalPos.y, targetCamY, Time.deltaTime * 10f);
        weaponStatHandler.playerCam.transform.localPosition = camLocalPos;

        if (weaponStatHandler.isADS)
            WeaponShake();
    }
    void WeaponShake()//손떨림
    {
        float accuracy = Mathf.Clamp01((99f - accuracyAmount) / 98f);
        float shakeAmount = accuracy * 7.5f;
        float shakeSpeed = 0.7f;

        float rotX = (Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) - 0.5f) * shakeAmount;
        float rotY = (Mathf.PerlinNoise(0f, Time.time * shakeSpeed) - 0.5f) * shakeAmount * 3f;
        float rotZ = (Mathf.PerlinNoise(Time.time * shakeSpeed, Time.time * shakeSpeed) - 0.5f) * shakeAmount;

        Quaternion shakeRotation = Quaternion.Euler(rotX, rotY, rotZ);
        weaponStatHandler.handransform.localRotation = initialLocalRotation * shakeRotation;
    }


    #endregion

    #region 발사 관련

    public void FireWeapon()
    {
        weaponStatHandler.lastFireTime = Time.time;
        if (currentAmmo > 0)
        {
            if (currentAmmo != 1)
            {
                weaponStatHandler.gunAnimator?.SetTrigger("Fire");
            }
            else if (currentAmmo == 1)
            {
                weaponStatHandler.gunAnimator?.SetBool("OutOfAmmo", true);
            }
            else
            {
                weaponStatHandler.gunAnimator?.SetBool("OutOfAmmo", true);
            }
            ShootRay();
            EjectCasing();
            MuzzleFlash();
            ApplyRecoil();
            SoundManager.Instance.PlaySFX(fireSound);

            currentAmmo--;

            //weaponStatHandler.onAmmoChanged?.Invoke(currentAmmo, weaponStatHandler.MaxAmmo);
        }
        else
        {
            SoundManager.Instance.PlaySFX(emptyTrigger);
        }
    }

    void ShootRay()
    {
        Vector3 shootDirection;

        if (weaponStatHandler.isADS)
        {
            shootDirection = weaponStatHandler.barrelLocation.forward;
        }
        else
        {
            float randomYaw = Random.Range(-weaponStatHandler.spreadAngle, weaponStatHandler.spreadAngle);//탄퍼짐 범위
            float randomPitch = Random.Range(-weaponStatHandler.spreadAngle, weaponStatHandler.spreadAngle);
            Quaternion spreadRot = Quaternion.Euler(randomPitch, randomYaw, 0f);
            shootDirection = spreadRot * weaponStatHandler.barrelLocation.forward;
        }

        Ray ray = new Ray(weaponStatHandler.barrelLocation.position, shootDirection);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {

            Quaternion hitRotation = Quaternion.LookRotation(hit.normal);
            GameObject impact = Instantiate(bulletImpactPrefab, hit.point, hitRotation);
            impact.transform.SetParent(hit.collider.transform);
            Destroy(impact, 5f);


            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Target"))
            {
                Target target = hit.collider.GetComponentInParent<Target>();
                target?.TakeDamage(weaponStatHandler.DMG, hit.collider);
            }
        }
    }
    void OnDrawGizmos()
    {
        if (weaponStatHandler == null || weaponStatHandler.barrelLocation == null)
            return;

        Gizmos.color = Color.yellow;

        Vector3 origin = weaponStatHandler.barrelLocation.position;
        Vector3 forward = weaponStatHandler.barrelLocation.forward;

        // 가운데 방향선
        Gizmos.DrawRay(origin, forward * 5f);

        // spreadAngle 기준으로 몇 개의 방향선 표시
        float spread = weaponStatHandler.spreadAngle;

        for (int i = 0; i < 8; i++)
        {
            float yaw = Random.Range(-spread, spread);
            float pitch = Random.Range(-spread, spread);
            Quaternion spreadRot = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 dir = spreadRot * forward;

            Gizmos.DrawRay(origin, dir * 5f);
        }
    }


    void MuzzleFlash()
    {

        GameObject flash = Instantiate(muzzleFlashPrefab, weaponStatHandler.barrelLocation.position, weaponStatHandler.barrelLocation.rotation);
        flash.transform.SetParent(weaponStatHandler.barrelLocation);
        Destroy(flash, weaponStatHandler.destroyTimer);

    }

    void EjectCasing()
    {
        if (weaponStatHandler.casingExitLocation)
        {
            GameObject casing = Instantiate(casingPrefab, weaponStatHandler.casingExitLocation.position, weaponStatHandler.casingExitLocation.rotation);
            Rigidbody rb = casing.GetComponent<Rigidbody>();
            if (rb != null)
            {
                weaponStatHandler.ejectPower = weaponStatHandler.DMG * 40f;
                float power = weaponStatHandler.ejectPower;
                rb.AddExplosionForce(Random.Range(power * 0.7f, power),
                    weaponStatHandler.casingExitLocation.position - weaponStatHandler.casingExitLocation.right * 0.3f - weaponStatHandler.casingExitLocation.up * 0.6f, 1f);
                rb.AddTorque(new Vector3(0, Random.Range(100, 500), Random.Range(100, 1000)), ForceMode.Impulse);
            }

            Destroy(casing, weaponStatHandler.destroyTimer);
        }
    }

    void CalculateFinalRecoil()
    {
        float rcl = player.Data.RCL;
        finalRecoil = weaponStatHandler.ShootRecoil * (0.2f + (0.8f * (1 - rcl / 99f)));
        //finalRecoil = baseRecoil * (1f - weaponStatHandler.itemRecoil * 0.01f);
        Debug.Log($"무기 반동:{weaponStatHandler.ShootRecoil}, 플레이어 반동제어:{rcl}, 최종 반동:{finalRecoil},");
    }

    void ApplyRecoil()
    {
        CalculateFinalRecoil();
        weaponStatHandler.fpsCamera?.ApplyRecoil(finalRecoil);
    }
    #endregion

    #region 장전

    public void ReloadWeapon()
    {
        if (currentAmmo == weaponStatHandler.MaxAmmo && weaponStatHandler.isADS)
        {
            return;
        }

        weaponStatHandler.isReloading = true;
        currentAmmo = 0;
        weaponStatHandler.gunAnimator.SetTrigger("Reload");

        SoundManager.Instance.PlaySFX(reloadSound);

        StartCoroutine(ReloadCoroutine());
    }

    IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(weaponStatHandler.ReloadTime);

        weaponStatHandler.gunAnimator.SetBool("OutOfAmmo", false);

        currentAmmo = weaponStatHandler.MaxAmmo;
        //weaponStatHandler.onAmmoChanged(currentAmmo, weaponStatHandler.MaxAmmo);
        weaponStatHandler.isReloading = false;
    }

    #endregion
}
