using UnityEngine;
using UnityEngine.UI;

public abstract class BaseTarget : MonoBehaviour
{
    [Header("타겟 데이터")]
    [SerializeField] protected TargetSO data;
    protected float currentHp;
    [SerializeField] protected Animator anim;

    [Header("사운드")]
    [SerializeField] protected AudioClip upSound;
    [SerializeField] protected AudioClip downSound;

    [Header("UI")]
    [SerializeField] protected Image hpBar;
    [SerializeField] protected GameObject targetUI;
    [SerializeField] protected GameObject blip;
    [SerializeField] protected Text lvText;


    protected virtual void Start()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>() ?? GetComponentInChildren<Animator>();
        }
    }


    private void Update()
    {
        if (StageManager.Instance.Player && currentHp > 0)
        {
            var origin = transform.eulerAngles;
            transform.LookAt(StageManager.Instance.Player.transform);
            transform.rotation = Quaternion.Euler(new Vector3(origin.x, transform.rotation.eulerAngles.y, origin.z));
        }
    }

    public abstract void TakeDamage(float amount, Collider hitCollider, Vector3 hitDirection);

    public void OnPlayerEnteredRange()
    {
        if (currentHp <= 0) return;
        if (anim != null)
        {
            if (!anim.GetBool("Up")) // 안 올라가있을 때만
            {
                anim.SetBool("Up", true);
                SoundManager.Instance.PlaySFXForClip(upSound, gameObject.transform.position);
            }
        }
    }

    protected virtual void Die()
    {
        if (anim != null)
        {
            anim.SetBool("Die", true);
        }

        blip.SetActive(false);
        targetUI.SetActive(false);
        SoundManager.Instance.PlaySFXForClip(downSound, gameObject.transform.position);
        Invoke(nameof(DeactivateTarget), 2f);

    }

    public virtual void InitData(TargetSO data)
    {
        ActivateAll();

        this.data = data;
        currentHp = data.Hp;
        lvText.text = $"{data.Level}";

        if (hpBar != null)
        {
            hpBar.fillAmount = 1f;
        }
    }

    private void DeactivateTarget()
    {
        gameObject.SetActive(false);
    }

    private void ActivateAll()
    {
        gameObject.SetActive(true);

        if (anim != null)
        {
            anim.SetBool("Die", false);
        }

        blip.SetActive(true);
        targetUI.SetActive(true);
    }
}
