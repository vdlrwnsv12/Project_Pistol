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
        Debug.Log($"타겟 초기화 ID: {data.ID}, Type: {data.Type}, HP: {data.Hp}");
    }

    public void TakeDamage(float amount, Collider hitCollider)
    {
        if (hitCollider != null && hitCollider.name == "Head")
        {
            amount = Mathf.RoundToInt(amount * 1.6f);
            Debug.Log($"헤드샷 데미지: {amount}");
        }

        currentHp -= amount;

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        anim.SetBool("Die", true);
    }
}
