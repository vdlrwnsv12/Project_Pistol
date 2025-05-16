using UnityEngine;

public class CivilianTarget : BaseTarget
{
    protected override void Start()
    {
        base.Start();
    }

    public override void TakeDamage(float amount, Collider hitCollider, Vector3 hitDirection)
    {
        if (currentHp <= 0) return;

        anim.SetTrigger("Hit");

        amount *= data.DamageRate;
        Debug.Log($"데미지: {amount}");

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
