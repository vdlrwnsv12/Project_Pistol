using UnityEngine;

public class CivilianTarget : BaseTarget
{
    [SerializeField]private LandTarget landTarget;
    
    //ToDo: 시민 타겟이 죽으면 하위 랜드 타겟도 죽게
    //하위 랜드 타겟만 죽으면 그냥 두기
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
            landTarget.CivilianTargetDie();
        }
    }
}
