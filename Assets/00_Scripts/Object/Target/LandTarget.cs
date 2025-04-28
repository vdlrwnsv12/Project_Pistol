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
        
        StageManager.Instance.HitCount++;
        bool isHeadShot;

        if (hitCollider != null && hitCollider.name == "Head")
        {
            amount = Mathf.RoundToInt(amount * data.DamageRate * 1.2f);
            Debug.Log($"헤드샷 데미지: {amount}");
            isHeadShot = true;
        }
        else
        {
            amount *= data.DamageRate;
            Debug.Log($"바디샷 데미지: {amount}");
            isHeadShot = false;
        }

        float realDamage = Mathf.Min(amount, currentHp);
        currentHp -= realDamage;
        Debug.Log($"{data.Name} 받은 데미지: {realDamage}");
        
        StageManager.Instance.GameScore += (int)(BaseScore(isHeadShot, realDamage) + RangeScore() + ComboScore(0) + QuickShotScore(StageManager.Instance.IsQuickShot));
        Debug.Log($"기본 점수: {BaseScore(isHeadShot, realDamage)}");
        Debug.Log($"원거리 점수: {RangeScore()}");
        Debug.Log($"콤보 점수: {ComboScore(0)}");
        Debug.Log($"큇 샷 점수: {QuickShotScore(StageManager.Instance.IsQuickShot)}");
        StageManager.Instance.IsQuickShot = true;

        hpBar.fillAmount = currentHp / data.Hp;

        if (currentHp <= 0)
        {
            Die();
        }
    }
    
    private float BaseScore(bool isHeadShot, float dmg)
    {
        return isHeadShot ? dmg * 1.5f : dmg;
    }

    private float RangeScore()
    {
        var dis = Vector3.Distance(StageManager.Instance.Player.transform.position, transform.position);
        return dis >= 2f ? 200 + Mathf.Floor((dis - 2f) / 0.1f) * 10f : 0;
    }

    private float ComboScore(int combo)
    {
        return combo >= 2 ? Mathf.Min(combo * 100f, 900f) : 0f;
    }

    private float QuickShotScore(bool isQuickShot)
    {
        return isQuickShot ? 500f : 0f;
    }
}
