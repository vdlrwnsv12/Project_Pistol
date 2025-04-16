using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("타겟 데이터")]
    public TargetDatas data; // 프리팹에 직접 넣는 방식
    public float currentHp;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

        if (data == null)
        {
            Debug.LogError("TargetData 안 넣음!");
            return;
        }

        currentHp = data.Hp;
    }

    public void TakeDamage(float amount, Collider hitCollider)
    {
        if (hitCollider != null && hitCollider.name == "Head")
        {
            amount = Mathf.RoundToInt(amount * data.DamageRate * 1.2f);
            Debug.Log($"헤드샷 데미지: {amount}");
        }else
        {
            amount = amount * data.DamageRate;
            Debug.Log($"바디샷 데미지{amount}");
        }        

        float realDamage = Mathf.Min(amount, currentHp);

        Debug.Log($"표적이 받은 데미지: {realDamage}");

        currentHp = Mathf.Max(currentHp - realDamage, 0);

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        anim.SetBool("Die", true);
        this.enabled = false;
    }
}
