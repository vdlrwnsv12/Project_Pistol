using UnityEngine;
using System.Collections;

public class WeaponFireController : MonoBehaviour
{
    private WeaponSO weaponData;
    private WeaponStatHandler statHandler;
    [SerializeField] private int currentAmmo;
    private Quaternion initialLocalRotation;
    private Vector3 camRootOriginPos;
    private Vector3 currentCamRootTargetPos;
    private Quaternion currentHandTargetRot;
    public float finalRecoil;

    public GameObject testUi;
    public bool isLocked = true;

    [SerializeField] private float targetCamY = 0.165f;


    #region Unity Methods

    public void InitReferences()
    {
        statHandler = GetComponent<WeaponStatHandler>();

        if (statHandler.weaponData == null)
        {
            string nameToSerch = gameObject.name.Replace("(Clone)", "").Trim();
            statHandler.weaponData = Resources.Load<WeaponSO>($"Data/SO/WeaponSO/{nameToSerch}");
            if (statHandler.weaponData == null)
            {
                Debug.Log($"[InitReferences] WeaponData '{nameToSerch}'을(를) 찾을 수 없습니다.");
            }
            else
            {
                Debug.Log($"[InitReferences] WeaponData '{nameToSerch}'자동 할당.");
            }
        }
        weaponData = statHandler.weaponData;
        initialLocalRotation = statHandler.handransform.localRotation;
        camRootOriginPos = statHandler.camRoot.localPosition;
        statHandler.playerObject.GetComponent<Player>().SetWeaponStatHandler(statHandler);
        currentAmmo = weaponData.MaxAmmo;
    }

    void Update()
    {
        if (statHandler == null)
        {
            return;
        }

        if (Input.GetButtonDown("Fire1") && isLocked)
        {
            FireWeapon();
            //statHandler.ToggleAttachment(statHandler.redDot);//아이템 얻으면 이거 호출해야함 조만간 빼야함
        }

        if (Input.GetKeyDown(KeyCode.R) && !statHandler.isReloading)
        {
            ReloadWeapon();
            //statHandler.ToggleAttachment(statHandler.laserPointer);//이것도 빼야함
        }
        if (Input.GetKeyDown(KeyCode.F))//테스트용 코드
        {
            if (isLocked)
                UnlockCursor();
            else
                LockCursor();
        }
        #region 레이저 포인터 테스트 삭제해야함
        if (statHandler.laserPointer.activeSelf == true)
        {
            statHandler.spreadAngle = 0;
        }
        else
        {
            statHandler.spreadAngle = 10.5f;
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


        if (Input.GetMouseButtonDown(1) && !statHandler.isReloading)
        {
            statHandler.isADS = !statHandler.isADS;
        }
        if (statHandler.isADS)
        {
            currentCamRootTargetPos = statHandler.adsPosition;
            currentHandTargetRot = initialLocalRotation;

            // redDot 상태에 따라 타겟 Y 설정
            targetCamY = (statHandler.redDot != null && statHandler.redDot.activeSelf) ? 0.18f : 0.16f;
        }
        else
        {
            currentCamRootTargetPos = camRootOriginPos;
            currentHandTargetRot = initialLocalRotation;

            // 정조준 해제 시 기본값으로 복구
            targetCamY = 0.16f;
        }

        // FOV 보간
        float targetFOV = statHandler.isADS ? 40f : 60f;
        statHandler.playerCam.fieldOfView = Mathf.Lerp(statHandler.playerCam.fieldOfView, targetFOV, Time.deltaTime * 10f);

        // 위치/회전 보간
        statHandler.camRoot.localPosition = Vector3.Lerp(statHandler.camRoot.localPosition, currentCamRootTargetPos, Time.deltaTime * statHandler.camMoveSpeed);
        statHandler.handransform.localRotation = Quaternion.Lerp(statHandler.handransform.localRotation, currentHandTargetRot, Time.deltaTime * 10f);

        //Y 위치만 따로 부드럽게 보간
        Vector3 camLocalPos = statHandler.playerCam.transform.localPosition;
        camLocalPos.y = Mathf.Lerp(camLocalPos.y, targetCamY, Time.deltaTime * 10f);
        statHandler.playerCam.transform.localPosition = camLocalPos;

        if (statHandler.isADS)
            WeaponShake();
    }

    void WeaponShake()//손떨림
    {
        float accuracyAmount = statHandler.playerObject.GetComponent<Player>().Data.HDL;
        float accuracy = Mathf.Clamp01((99f - accuracyAmount) / 98f);
        float shakeAmount = accuracy * 7.5f;
        float shakeSpeed = 0.7f;

        float rotX = (Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) - 0.5f) * shakeAmount;
        float rotY = (Mathf.PerlinNoise(0f, Time.time * shakeSpeed) - 0.5f) * shakeAmount * 3f;
        float rotZ = (Mathf.PerlinNoise(Time.time * shakeSpeed, Time.time * shakeSpeed) - 0.5f) * shakeAmount;

        Quaternion shakeRotation = Quaternion.Euler(rotX, rotY, rotZ);
        statHandler.handransform.localRotation = initialLocalRotation * shakeRotation;
    }

    #endregion

    #region 발사 관련

    void FireWeapon()
    {
        statHandler.lastFireTime = Time.time;

        if (weaponData == null)
        {
            return;
        }

        if (currentAmmo > 0)
        {
            if (currentAmmo != 1)
            {
                statHandler.gunAnimator?.SetTrigger("Fire");
            }
            else if (currentAmmo == 1)
            {
                statHandler.gunAnimator?.SetBool("OutOfAmmo", true);
            }
            else
            {
                statHandler.gunAnimator?.SetBool("OutOfAmmo", true);
            }
            ShootRay();
            EjectCasing();
            MuzzleFlash();
            ApplyRecoil();
            SoundManager.Instance.PlaySFX(statHandler.fireSound);

            currentAmmo--;
        }
        else
        {
            SoundManager.Instance.PlaySFX(statHandler.emptySound);
        }
    }

    void ShootRay()
    {
        Vector3 shootDirection;

        if (statHandler.isADS)
        {
            shootDirection = statHandler.barrelLocation.forward;
        }
        else
        {
            float randomYaw = Random.Range(-statHandler.spreadAngle, statHandler.spreadAngle);//탄퍼짐 범위
            float randomPitch = Random.Range(-statHandler.spreadAngle, statHandler.spreadAngle);
            Quaternion spreadRot = Quaternion.Euler(randomPitch, randomYaw, 0f);
            shootDirection = spreadRot * statHandler.barrelLocation.forward;
        }

        Ray ray = new Ray(statHandler.barrelLocation.position, shootDirection);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (statHandler.bulletImpactPrefab)
            {
                Quaternion hitRotation = Quaternion.LookRotation(hit.normal);
                GameObject impact = Instantiate(statHandler.bulletImpactPrefab, hit.point, hitRotation);
                impact.transform.SetParent(hit.collider.transform);
                Destroy(impact, 2f);
            }

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Target"))
            {
                Target target = hit.collider.GetComponentInParent<Target>();
                target?.TakeDamage(weaponData.DMG, hit.collider);
            }
        }
        StartCoroutine(CameraShake(weaponData.DMG * 0.0125f));
    }
    void OnDrawGizmos()
    {
        if (statHandler == null || statHandler.barrelLocation == null)
            return;

        Gizmos.color = Color.yellow;

        Vector3 origin = statHandler.barrelLocation.position;
        Vector3 forward = statHandler.barrelLocation.forward;

        // 가운데 방향선
        Gizmos.DrawRay(origin, forward * 5f);

        // spreadAngle 기준으로 몇 개의 방향선 표시
        float spread = statHandler.spreadAngle;

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
        if (statHandler.muzzleFlashPrefab)
        {
            GameObject flash = Instantiate(statHandler.muzzleFlashPrefab, statHandler.barrelLocation.position, statHandler.barrelLocation.rotation);
            flash.transform.SetParent(statHandler.barrelLocation);
            Destroy(flash, statHandler.destroyTimer);
        }
    }

    void EjectCasing()
    {
        if (statHandler.casingPrefab && statHandler.casingExitLocation)
        {
            GameObject casing = Instantiate(statHandler.casingPrefab, statHandler.casingExitLocation.position, statHandler.casingExitLocation.rotation);
            Rigidbody rb = casing.GetComponent<Rigidbody>();
            if (rb != null)
            {
                statHandler.ejectPower = statHandler.weaponData.DMG * 40f;
                float power = statHandler.ejectPower;
                rb.AddExplosionForce(Random.Range(power * 0.7f, power),
                    statHandler.casingExitLocation.position - statHandler.casingExitLocation.right * 0.3f - statHandler.casingExitLocation.up * 0.6f, 1f);
                rb.AddTorque(new Vector3(0, Random.Range(100, 500), Random.Range(100, 1000)), ForceMode.Impulse);
            }

            Destroy(casing, statHandler.destroyTimer);
        }
    }

    void CalculateFinalRecoil()
    {
        float rcl = statHandler.playerObject.GetComponent<Player>().Data.RCL;
        finalRecoil = weaponData.ShootRecoil * (0.2f + (0.8f * (1 - rcl / 99f)));
    }

    void ApplyRecoil()
    {
        CalculateFinalRecoil();
        statHandler.fpsCamera?.ApplyRecoil(finalRecoil);
    }

    IEnumerator CameraShake(float intensity)
    {
        Vector3 originalPos = statHandler.playerObject.transform.localPosition;
        float duration = 0.25f;
        float timer = 0f;

        while (timer < duration)
        {
            float damper = 1f - (timer / duration);
            float x = Random.Range(-1f, 1f) * intensity * damper;
            float y = Random.Range(-1f, 1f) * intensity * damper;

            statHandler.playerObject.transform.localPosition = originalPos + new Vector3(x, y, 0f);

            timer += Time.deltaTime;
            yield return null;
        }

        statHandler.playerObject.transform.localPosition = originalPos;
    }

    #endregion

    #region 장전

    public void ReloadWeapon()
    {
        if (currentAmmo == weaponData.MaxAmmo && statHandler.isADS)
        {
            return;
        }

        statHandler.isReloading = true;
        currentAmmo = 0;
        statHandler.gunAnimator.SetTrigger("Reload");

        SoundManager.Instance.PlaySFX(statHandler.reloadSound);

        StartCoroutine(ReloadCoroutine());
    }

    IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(weaponData.ReloadTime);

        statHandler.gunAnimator.SetBool("OutOfAmmo", false);

        currentAmmo = weaponData.MaxAmmo;
        statHandler.isReloading = false;
    }

    #endregion
}
