using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirialTarget : BaseTarget
{
    private Vector3 hitDirection;
    private Rigidbody rb;
    [SerializeField]private GameObject targetObj;
    protected override void Start()
    {
        base.Start();

        rb = targetObj.GetComponent<Rigidbody>();
        if(rb == null)
        {
            rb = targetObj.gameObject.AddComponent<Rigidbody>();
        }
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    public override void TakeDamage(float amount, Collider hitCollider, Vector3 hitDirection)
    {
        if (currentHp <= 0) return;

        if(anim != null)
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

    private void AddForceTarget()
    {
        if (rb != null)
        {
            
            rb.useGravity = true;
            rb.isKinematic = false;

            rb.AddForce(hitDirection * 5f + Vector3.up, ForceMode.Impulse);
            rb.AddTorque(Random.onUnitSphere * 5f, ForceMode.Impulse);
        }
    }
}
