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
    [SerializeField] protected GameObject blip;


    protected virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        var origin = transform.eulerAngles;
        transform.LookAt(StageManager.Instance.Player.transform);
        transform.rotation = Quaternion.Euler(new Vector3(origin.x, transform.rotation.eulerAngles.y - 90f, origin.z));
    }

    public abstract void TakeDamage(float amount, Collider hitCollider);

    public void OnPlayerEnteredRange()
    {
        if (currentHp <= 0) return;

        if (!anim.GetBool("Up")) // 안 올라가있을 때만
        {
            anim.SetBool("Up", true);
            SoundManager.Instance.PlaySFXForClip(upSound);
        }
    }

    protected virtual void Die()
    {
        anim.SetBool("Die", true);
        Destroy(blip);
        SoundManager.Instance.PlaySFXForClip(downSound);
    }

    public void InitData(TargetSO data)
    {
        this.data = data;
        
        currentHp = data.Hp;
        lvText.text = $"{data.Level}";

        if (hpBar != null)
        {
            hpBar.fillAmount = 1f;
        }
    }
}
