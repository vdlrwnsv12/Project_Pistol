using Unity.VisualScripting;
using UnityEngine;

public class AerialTarget : BaseTarget
{
    private Vector3 hitDirection;
    private Rigidbody rb;
    [SerializeField] private GameObject targetObj;
    [SerializeField] private Transform targetPoint;
    protected override void Start()
    {
        base.Start();

        rb = targetObj.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = targetObj.gameObject.AddComponent<Rigidbody>();
        }
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    public override void TakeDamage(float amount, Collider hitCollider, Vector3 hitDirection)
    {
        if (currentHp <= 0) return;

        if (anim != null)
        {
            anim.SetTrigger("Hit");
        }

        if (hitCollider != null && hitCollider.name == "Head")
        {
            amount = Mathf.RoundToInt(amount * data.DamageRate * 1.2f);
            Debug.Log($"헤드샷 데미지: {amount}");
        }
        else
        {
            amount *= data.DamageRate;
            Debug.Log($"바디샷 데미지: {amount}");
        }

        float realDamage = Mathf.Min(amount, currentHp);
        currentHp -= realDamage;
        Debug.Log($"{data.Name} 받은 데미지: {realDamage}");

        hpBar.fillAmount = currentHp / data.Hp;

        if (currentHp <= 0)
        {
            this.hitDirection = hitDirection;
            Die();
            AddForceTarget();
        }
    }


    private void AddForceTarget() //타겟이 총알의 방향대로 날아감
    {
        if (rb != null)
        {

            rb.useGravity = true;
            rb.isKinematic = false;

            rb.AddForce(hitDirection * 5f + Vector3.up, ForceMode.Impulse);
            rb.AddTorque(Random.onUnitSphere * 5f, ForceMode.Impulse);
        }
    }

    public override void InitData(TargetSO data)
    {
        base.InitData(data);

        if (rb == null && targetObj != null)
        {
            rb = targetObj.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = targetObj.gameObject.AddComponent<Rigidbody>();
            }
        }

        rb.useGravity = false;
        rb.isKinematic = true;

        targetObj.transform.position = targetPoint.position;
        targetObj.transform.rotation = targetPoint.rotation;
    }
}
