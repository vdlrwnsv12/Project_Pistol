using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Xml;

public class Target : MonoBehaviour
{
    [Header("타겟 데이터")]
    [SerializeField]private TargetSO data; // 프리팹에 직접 넣는 방식
    private float currentHp;
    [SerializeField]private Animator anim;
    [SerializeField]private Text lvText;

    [Header("시민일 경우 메테리얼")]
    [SerializeField]private Material civilianMaterial;

    [Header("사운드")]
    [SerializeField]private AudioClip upSound;
    [SerializeField]private AudioClip downSound;

    [Header("UI")]
    [SerializeField]private Image hpBar;
    [SerializeField]private GameObject targetUI;

    void Awake()
    {
        if (data == null)
        {
            TargetSO[] allDatas = Resources.LoadAll<TargetSO>("Data/SO/TargetSO");
            if (allDatas.Length > 0)
            {
                data = allDatas[Random.Range(0, allDatas.Length)];
            }
            return;
        }
    }
    void Start()//데이터 종류 별 메테리얼 변경
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
        lvText.text = $"{data.Level}";

        if(hpBar != null)
        {
            hpBar.fillAmount = 1f;
        }
    }

    public void TakeDamage(float amount, Collider hitCollider)
    {
        if (currentHp <= 0) return;

        anim.SetTrigger("Hit");

        if (data.Name != "Civilian" && hitCollider != null && hitCollider.name == "Head")
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
        Destroy(targetUI);
        anim.SetBool("Die", true);
        SoundManager.Instance.PlaySFX(downSound);
    }
}
