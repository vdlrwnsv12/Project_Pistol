using UnityEngine;
using UnityEngine.UI;

public abstract class BaseTarget : MonoBehaviour
{
    [Header("타겟 데이터")]
    [SerializeField] protected TargetSO data;
    protected float currentHp;
    [SerializeField] protected Animator anim;
    [SerializeField] protected Text lvText;

    [Header("사운드")]
    [SerializeField] protected AudioClip upSound;
    [SerializeField] protected AudioClip downSound;

    [Header("UI")]
    [SerializeField] protected Image hpBar;
    [SerializeField] protected GameObject targetUI;

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        currentHp = data.Hp;
        lvText.text = $"{data.Level}";

        if (hpBar != null)
        {
            hpBar.fillAmount = 1f;
        }
    }

    public abstract void TakeDamage(float amount, Collider hitCollider);

    public void OnPlayerEnteredRange()
    {
        if (currentHp <= 0) return;

        if (!anim.GetBool("Up")) // 안 올라가있을 때만
        {
            anim.SetBool("Up", true);
            SoundManager.Instance.PlaySFX(upSound);
        }
    }

    protected virtual void Die()
    {
        Destroy(targetUI);
        anim.SetBool("Die", true);
        SoundManager.Instance.PlaySFX(downSound);
    }
}
