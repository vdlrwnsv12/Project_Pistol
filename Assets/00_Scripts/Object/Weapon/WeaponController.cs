using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private Weapon weapon;
    private WeaponStatHandler stat;
    private int curAmmo;
    private float lastFireTime;
    [HideInInspector] public bool isReloading;

    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    private GameObject muzzleFlashPrefab;
    private GameObject casingPrefab;
    private GameObject bulletImpactPrefab;

    private float ejectPower = 150f;

    public int CurAmmo => curAmmo;

    private void Awake()
    {
        weapon = GetComponent<Weapon>();

        muzzleFlashPrefab = ResourceManager.Instance.Load<GameObject>("Prefabs/FX/MuzzleFlash");
        casingPrefab = ResourceManager.Instance.Load<GameObject>("Prefabs/Props/Case");
        bulletImpactPrefab = ResourceManager.Instance.Load<GameObject>("Prefabs/FX/BulletHole");

        isReloading = false;
    }

    private void Start()
    {
        stat = weapon.Stat;
        curAmmo = stat.MaxAmmo;
    }

    #region 발사

    public void Fire(bool isAds)
    {
        lastFireTime = Time.time;
        if (lastFireTime < 1f)
        {
            return;
        }

        if (curAmmo > 0)
        {
            if (curAmmo != 1)
            {
                weapon.Anim.SetTrigger("Fire");
            }
            else if (curAmmo == 1)
            {
                weapon.Anim.SetBool("OutOfAmmo", true);
            }
            else
            {
                weapon.Anim.SetBool("OutOfAmmo", true);
            }

            ShootRay(isAds);
            EjectCasing();
            MuzzleFlash();

            StageManager.Instance.ShotCount++;
            SoundManager.Instance.PlaySFX(weapon.Data.name);

            curAmmo--;
        }
        else
        {
            // 탄창 없을 경우
            SoundManager.Instance.PlaySFX("EmptyTrigger");
        }

        lastFireTime = 0f;
    }

    private void ShootRay(bool isAds)
    {
        Vector3 shootDirection;

        shootDirection = barrelLocation.forward;

        Ray ray = new Ray(barrelLocation.position, shootDirection);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // 탄흔 처리
            if (bulletImpactPrefab)
            {
                Quaternion hitRotation = Quaternion.LookRotation(hit.normal);
                GameObject impact = ObjectPoolManager.Instance.GetObject(bulletImpactPrefab, hit.point, hitRotation, 5f);
                impact.transform.SetParent(hit.collider.transform);

                // AutoReturn 처리
                //ObjectPoolManager.Instance.AutoReturnToPool(impact, 5f);  // 5초 후 반환
            }

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Target"))
            {
                BaseTarget target = hit.collider.GetComponentInParent<BaseTarget>();
                target?.TakeDamage(stat.Damage, hit.collider);
            }
            else
            {
                StageManager.Instance.DestroyTargetCombo = 0;
            }
        }
    }

    private void EjectCasing()
    {
        if (casingPrefab && casingExitLocation)
        {
            GameObject casing = ObjectPoolManager.Instance.GetObject(casingPrefab, casingExitLocation.position, casingExitLocation.rotation, 6f);

            Rigidbody rb = casing.GetComponent<Rigidbody>();
            if (rb != null)
            {
                ejectPower = stat.Damage * 40f;
                float power = ejectPower;

                rb.AddExplosionForce(Random.Range(power * 0.7f, power),
                    casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f,
                    1f);

                rb.AddTorque(new Vector3(0, Random.Range(100, 500), Random.Range(100, 1000)), ForceMode.Impulse);
            }

            // 자동 반환 확인 후 삭제
            //ObjectPoolManager.Instance.AutoReturnToPool(casing, 5f);  // 5초 후 반환
        }
    }

    private void MuzzleFlash()
    {
        if (muzzleFlashPrefab)
        {
            GameObject flash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);
            flash.transform.SetParent(barrelLocation);
        }
    }

    #endregion

    #region 장전

    public void ReloadWeapon(bool isAds)
    {
        if (curAmmo == stat.MaxAmmo && isAds)
        {
            return;
        }

        curAmmo = 0;
        weapon.Anim.SetTrigger("Reload");

        SoundManager.Instance.PlaySFX("Reload");

        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(stat.ReloadTime);

        weapon.Anim.SetBool("OutOfAmmo", false);

        curAmmo = stat.MaxAmmo;
    }

    #endregion
}
