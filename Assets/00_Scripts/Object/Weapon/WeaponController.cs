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

    private PoolableResource impactPoolable;
    private PoolableResource casePoolable;

    public int CurAmmo => curAmmo;

    private void Awake()
    {
        weapon = GetComponent<Weapon>();

        muzzleFlashPrefab = ResourceManager.Instance.Load<GameObject>("Prefabs/FX/MuzzleFlash");
        casingPrefab = ResourceManager.Instance.Load<GameObject>("Prefabs/Props/Case");
        bulletImpactPrefab = ResourceManager.Instance.Load<GameObject>("Prefabs/FX/BulletHole");

        isReloading = false;

        impactPoolable = bulletImpactPrefab.GetComponent<PoolableResource>();
        casePoolable = casingPrefab.GetComponent<PoolableResource>();
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
            // 탄흔 처리: GetComponent 한 번만 호출
            if (bulletImpactPrefab)
            {
                if (impactPoolable != null)
                {
                    Quaternion hitRotation = Quaternion.LookRotation(hit.normal);
                    GameObject impact =
                        ObjectPoolManager.Instance.GetObjectInPool(impactPoolable, hit.point, hitRotation);
                    impact.transform.SetParent(hit.collider.transform);

                    // AutoReturn 처리
                    if (impactPoolable != null && impactPoolable.isAutoReturn)
                    {
                        ObjectPoolManager.Instance.AutoReturnToPool(impact, impactPoolable.returnTime);
                    }
                }
                else
                {
                    // fallback: 풀링 컴포넌트가 없으면 인스턴스화
                    Quaternion hitRotation = Quaternion.LookRotation(hit.normal);
                    GameObject impact = Instantiate(bulletImpactPrefab, hit.point, hitRotation);
                    impact.transform.SetParent(hit.collider.transform);
                    Destroy(impact, 5f);
                    Debug.LogWarning("PoolableResource가 없어 Instantiate 사용됨");
                }
            }

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Target"))
            {
                Target target = hit.collider.GetComponentInParent<Target>();
                target?.TakeDamage(stat.Damage, hit.collider);
            }
        }
    }

    private void EjectCasing()
    {
        if (casingPrefab && casingExitLocation)
        {
            if (casePoolable == null)
            {
                Debug.LogWarning("PoolableResource 컴포넌트가 casingPrefab에 없습니다.");
                return;
            }

            GameObject casing = ObjectPoolManager.Instance.GetObjectInPool(
                casePoolable,
                casingExitLocation.position,
                casingExitLocation.rotation
            );

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
            var poolableInstance = casing.GetComponent<PoolableResource>();
            if (poolableInstance != null && poolableInstance.isAutoReturn)
            {
                ObjectPoolManager.Instance.AutoReturnToPool(casing, poolableInstance.returnTime);
            }
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