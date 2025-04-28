using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandTarget : BaseTarget
{
    protected override void Start()
    {
        base.Start();
    }

    public override void TakeDamage(float amount, Collider hitCollider)
    {
        if (currentHp <= 0) return;

        anim.SetTrigger("Hit");

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
            Die();
        }
    }
}
