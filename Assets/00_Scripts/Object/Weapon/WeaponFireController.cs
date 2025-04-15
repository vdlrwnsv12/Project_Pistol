using UnityEngine;
using System.Collections;

public class WeaponFireController : MonoBehaviour
{
    private WeaponData weaponData;
    private WeaponStatHandler statHandler;

    private Quaternion initialLocalRotation;
    private Vector3 camRootOriginPos;
    private Vector3 currentCamRootTargetPos;
    private Quaternion currentHandTargetRot;

    public float finalRecoil;

    #region Unity Methods

    public void InitReferences()
    {
        statHandler = GetComponent<WeaponStatHandler>();
        weaponData = statHandler.weaponData;
        initialLocalRotation = statHandler.handransform.localRotation;
        camRootOriginPos = statHandler.camRoot.localPosition;
        statHandler.playerObject.GetComponent<Player>().SetWeaponStatHandler(statHandler);
    }

    void Update()
    {
        if (statHandler == null)
        {
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            FireWeapon();
        }

        if (Input.GetKeyDown(KeyCode.R) && !statHandler.isReloading)
        {
            ReloadWeapon();
        }

        HandleADS();
    }

    #endregion

    #region ADS

    void HandleADS()//정조준
    {
        if (Input.GetMouseButtonDown(1))
        {
            statHandler.isADS = !statHandler.isADS;

            if (statHandler.isADS)
            {
                currentCamRootTargetPos = statHandler.adsPosition;
                currentHandTargetRot = initialLocalRotation; // 흔들기 시작점
            }
            else
            {
                currentCamRootTargetPos = camRootOriginPos;
                currentHandTargetRot = initialLocalRotation;
            }
        }

        // FOV 보간
        float targetFOV = statHandler.isADS ? 40f : 60f;
        statHandler.playerCam.fieldOfView = Mathf.Lerp(statHandler.playerCam.fieldOfView, targetFOV, Time.deltaTime * 10f);

        // 위치/회전 보간
        statHandler.camRoot.localPosition = Vector3.Lerp(statHandler.camRoot.localPosition, currentCamRootTargetPos, Time.deltaTime * statHandler.camMoveSpeed);
        statHandler.handransform.localRotation = Quaternion.Lerp(statHandler.handransform.localRotation, currentHandTargetRot, Time.deltaTime * 10f);

        // 흔들림
        if (statHandler.isADS)
            WeaponShake();
    }

    void WeaponShake()//손떨림
    {
        float accuracyAmount = statHandler.playerObject.GetComponent<Player>().Data.hdl;
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

        if (weaponData.currentAmmo > 0)
        {
            if (statHandler.weaponData.currentAmmo != 1)
            {
                statHandler.gunAnimator?.SetTrigger("Fire");
            }
            else
            {
                statHandler.gunAnimator?.SetBool("OutOfAmmo", true);
            }
            ShootRay();
            EjectCasing();
            MuzzleFlash();
            ApplyRecoil();
            SoundManager.Instance.PlaySFX(statHandler.weaponData.fireSound);

            weaponData.currentAmmo--;
        }
        else
        {
            SoundManager.Instance.PlaySFX(statHandler.weaponData.emptySound);
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
            float randomYaw = Random.Range(-statHandler.spreadAngle, statHandler.spreadAngle);
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
                target?.TakeDamage(weaponData.damage, hit.collider);
            }
        }

        StartCoroutine(CameraShake(weaponData.cameraShakeRate * 0.005f));
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
                float power = statHandler.ejectPower;
                rb.AddExplosionForce(Random.Range(power * 0.7f, power),
                    statHandler.casingExitLocation.position - statHandler.casingExitLocation.right * 0.3f - statHandler.casingExitLocation.up * 0.6f, 1f);
                rb.AddTorque(new Vector3(0, Random.Range(100, 500), Random.Range(100, 1000)), ForceMode.Impulse);
            }

            Destroy(casing, statHandler.destroyTimer);
            SoundManager.Instance.PlaySFX(statHandler.weaponData.shellSound);
        }
    }

    void CalculateFinalRecoil()
    {
        float rcl = statHandler.playerObject.GetComponent<Player>().Data.rcl;
        finalRecoil = weaponData.shootRecoil * (0.2f + (0.8f * (1 - rcl / 99f)));
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
        if (weaponData.currentAmmo == weaponData.maxAmmo) 
        {
            return;
        }

        statHandler.isReloading = true;
        weaponData.currentAmmo = 0;
        statHandler.gunAnimator.SetTrigger("Reload");

        SoundManager.Instance.PlaySFX(statHandler.weaponData.reloadSound);

        StartCoroutine(ReloadCoroutine());
    }

    IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(weaponData.reloadTime);

        statHandler.gunAnimator.SetBool("OutOfAmmo", false);

        weaponData.currentAmmo = weaponData.maxAmmo;
        statHandler.isReloading = false;
    }

    #endregion
}
