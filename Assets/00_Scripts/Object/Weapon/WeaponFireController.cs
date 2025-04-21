using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponFireController : MonoBehaviour
{
    private WeaponSO weaponData;
    public WeaponStatHandler weaponStatHandler;
    [SerializeField] public int currentAmmo;
    private Vector3 camRootOriginPos;
    private Vector3 currentCamRootTargetPos;
    private Quaternion currentHandTargetRot;
     private Quaternion initialLocalRotation;

    public float finalRecoil;
    public bool isLocked = true;
    [SerializeField] private List<GameObject> optics;

    [SerializeField] private float targetCamY = 0.165f;
    [SerializeField] private float accuracyAmount;


    #region Unity Methods

    public void InitReferences()
    {
        weaponStatHandler = GetComponent<WeaponStatHandler>();

        if (weaponStatHandler.weaponData == null)
        {
            string nameToSerch = gameObject.name.Replace("(Clone)", "").Trim();
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
        weaponData = weaponStatHandler.weaponData;
        weaponStatHandler.WeaponDataFromSO();
        initialLocalRotation = weaponStatHandler.handransform.localRotation;
        camRootOriginPos = weaponStatHandler.camRoot.localPosition;
        currentAmmo = weaponStatHandler.MaxAmmo;
        weaponStatHandler.BindToWeapon(this);
        weaponStatHandler.onAmmoChanged(currentAmmo, weaponStatHandler.MaxAmmo);
        optics = new List<GameObject> { weaponStatHandler.redDot, weaponStatHandler.holographic };
        accuracyAmount = weaponStatHandler.playerObject.GetComponent<Player>().Data.HDL;
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
        HandleADS();
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

    void HandleADS()
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
        Debug.Log("총알 나감!");
        if (weaponData == null)
        {
            return;
        }

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
            SoundManager.Instance.PlaySFX(weaponStatHandler.fireSound);

            currentAmmo--;

            weaponStatHandler.onAmmoChanged?.Invoke(currentAmmo, weaponStatHandler.MaxAmmo);
        }
        else
        {
            SoundManager.Instance.PlaySFX(weaponStatHandler.emptySound);
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
            if (weaponStatHandler.bulletImpactPrefab)
            {
                Quaternion hitRotation = Quaternion.LookRotation(hit.normal);
                GameObject impact = Instantiate(weaponStatHandler.bulletImpactPrefab, hit.point, hitRotation);
                impact.transform.SetParent(hit.collider.transform);
                Destroy(impact, 5f);
            }

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Target"))
            {
                Target target = hit.collider.GetComponentInParent<Target>();
                target?.TakeDamage(weaponStatHandler.DMG, hit.collider);
            }
        }
        //StartCoroutine(CameraShake(weaponStatHandler.DMG * 0.0125f));
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
        if (weaponStatHandler.muzzleFlashPrefab)
        {
            GameObject flash = Instantiate(weaponStatHandler.muzzleFlashPrefab, weaponStatHandler.barrelLocation.position, weaponStatHandler.barrelLocation.rotation);
            flash.transform.SetParent(weaponStatHandler.barrelLocation);
            Destroy(flash, weaponStatHandler.destroyTimer);
        }
    }

    void EjectCasing()
    {
        if (weaponStatHandler.casingPrefab && weaponStatHandler.casingExitLocation)
        {
            GameObject casing = Instantiate(weaponStatHandler.casingPrefab, weaponStatHandler.casingExitLocation.position, weaponStatHandler.casingExitLocation.rotation);
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
        float rcl = weaponStatHandler.playerObject.GetComponent<Player>().Data.RCL;
        finalRecoil = weaponStatHandler.ShootRecoil * (0.2f + (0.8f * (1 - rcl / 99f)));
        //finalRecoil = baseRecoil * (1f - weaponStatHandler.itemRecoil * 0.01f);
        Debug.Log($"무기 반동:{weaponStatHandler.ShootRecoil}, 플레이어 반동제어:{rcl}, 최종 반동:{finalRecoil},");
    }

    void ApplyRecoil()
    {
        CalculateFinalRecoil();
        weaponStatHandler.fpsCamera?.ApplyRecoil(finalRecoil);
    }

    IEnumerator CameraShake(float intensity)
    {
        Vector3 originalPos = weaponStatHandler.playerObject.transform.localPosition;
        float duration = 0.25f;
        float timer = 0f;

        while (timer < duration)
        {
            float damper = 1f - (timer / duration);
            float x = Random.Range(-1f, 1f) * intensity * damper;
            float y = Random.Range(-1f, 1f) * intensity * damper;

            weaponStatHandler.playerObject.transform.localPosition = originalPos + new Vector3(x, y, 0f);

            timer += Time.deltaTime;
            yield return null;
        }

        weaponStatHandler.playerObject.transform.localPosition = originalPos;
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

        SoundManager.Instance.PlaySFX(weaponStatHandler.reloadSound);

        StartCoroutine(ReloadCoroutine());
    }

    IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(weaponStatHandler.ReloadTime);

        weaponStatHandler.gunAnimator.SetBool("OutOfAmmo", false);

        currentAmmo = weaponStatHandler.MaxAmmo;
        weaponStatHandler.onAmmoChanged(currentAmmo, weaponStatHandler.MaxAmmo);
        weaponStatHandler.isReloading = false;
    }

    #endregion
}
