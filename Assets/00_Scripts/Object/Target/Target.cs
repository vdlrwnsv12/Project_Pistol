using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour
{
    [Header("타겟 데이터")]
    public TargetSO data; // 프리팹에 직접 넣는 방식
    public float currentHp;
    public Animator anim;

    [Header("시민일 경우 메테리얼")]
    public Material civilianMaterial;

    [Header("사운드")]
    public AudioClip upSound;
    public AudioClip downSound;

    void Awake()
    {
        if (data == null)
        {
            TargetSO[] allDatas = Resources.LoadAll<TargetSO>("Data/SO/TargetSO");
            if (allDatas.Length > 0)
            {
                data = allDatas[Random.Range(0, allDatas.Length)];
                Debug.Log($"타겟 할당됨 {data.name}");
            }
            return;
        }
    }
    void Start()
    {
        anim = GetComponent<Animator>();
        if (data.Name == "Civilian")
        {
            Renderer rend = GetComponentInChildren<Renderer>();
            if (rend != null)
            {
                rend.material = civilianMaterial;
            }
        }
        else if (data.Name == "Airial")
        {
            Renderer rend = GetComponentInChildren<Renderer>();
            if (rend != null)
            {
                rend.material.color = Color.cyan;
            }
        }
        currentHp = data.Hp;
    }

    public void TakeDamage(float amount, Collider hitCollider)
    {
        if (currentHp <= 0) return;

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
        Debug.Log($"표적이 받은 데미지: {realDamage}");

        if (currentHp <= 0)
        {
            Die();
        }
    }

    public void OnPlayerEnteredRange()
    {
        if (currentHp <= 0) return;

        if (!anim.GetBool("Up")) // 안 올라가있을 때만
        {
            anim.SetBool("Up", true);
            SoundManager.Instance.PlaySFX(upSound);
        }
    }

    private void Die()
    {
        anim.SetBool("Die", true);
        SoundManager.Instance.PlaySFX(downSound);
    }
}
