using UnityEngine;

public class CivilianTarget : BaseTarget
{
    public override void TakeDamage(float amount, Collider hitCollider, Vector3 hitDirection)
    {
        if (currentHp <= 0) return;

        anim.SetTrigger("Hit");

        amount *= data.DamageRate;

        float realDamage = Mathf.Min(amount, currentHp);
        currentHp -= realDamage;

        //hpBar.fillAmount = currentHp / data.Hp;

        StageManager.Instance.DestroyTargetCombo = 0;
        StageManager.Instance.GameScore -= (int)data.Hp;
        AnalyticsManager.Instance.roomCombo = 0;
        AnalyticsManager.Instance.roomScore -= (int)data.Hp;
        if (currentHp <= 0)
        {
            Die();
        }
    }
}
