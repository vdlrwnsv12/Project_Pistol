using System.Collections;
using test;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private Weapon weapon;
    private test.WeaponStatHandler stat;
    private int curAmmo;
    private float lastFireTime;
    [HideInInspector] public bool isReloading;
    
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    private GameObject muzzleFlashPrefab;
    private GameObject casingPrefab;
    private GameObject bulletImpactPrefab;
    
    [Header("Settings")]
    [SerializeField, Range(0f, 20f)]
    private float spreadAngle = 10.5f;
    private float ejectPower = 150f;
    
    public int CurAmmo => curAmmo;

    private void Awake()
    {
        weapon = GetComponent<Weapon>();

        muzzleFlashPrefab = ResourceManager.Instance.Load<GameObject>("Prefabs/FX/MuzzleFlash");
        casingPrefab = ResourceManager.Instance.Load<GameObject>("Prefabs/Props/45ACP Bullet_Casing");
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

            //TODO: 사격 사운드 출력
            
            curAmmo--;
        }
        else
        {
            // 탄창 없을 경우
            //TODO: 탄 없는 사운드 출력
        }

        lastFireTime = 0f;
    }

    private void ShootRay(bool isAds)
    {
        Vector3 shootDirection;

        if (isAds)
        {
            shootDirection = barrelLocation.forward;
        }
        else
        {
            float randomYaw = Random.Range(-spreadAngle, spreadAngle);//탄퍼짐 범위
            float randomPitch = Random.Range(-spreadAngle, spreadAngle);
            Quaternion spreadRot = Quaternion.Euler(randomPitch, randomYaw, 0f);
            shootDirection = spreadRot * barrelLocation.forward;
        }

        Ray ray = new Ray(barrelLocation.position, shootDirection);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (bulletImpactPrefab)
            {
                Quaternion hitRotation = Quaternion.LookRotation(hit.normal);
                GameObject impact = Instantiate(bulletImpactPrefab, hit.point, hitRotation);
                impact.transform.SetParent(hit.collider.transform);
                Destroy(impact, 5f);
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
            GameObject casing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation);
            Rigidbody rb = casing.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //TODO: 이건 뭐지???
                ejectPower = stat.Damage * 40f;
                float power = ejectPower;
                
                rb.AddExplosionForce(Random.Range(power * 0.7f, power),
                    casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f, 1f);
                rb.AddTorque(new Vector3(0, Random.Range(100, 500), Random.Range(100, 1000)), ForceMode.Impulse);
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

        //SoundManager.Instance.PlaySFX(weaponStatHandler.reloadSound);

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