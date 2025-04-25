using UnityEngine;

[RequireComponent(typeof(WeaponController), typeof(Animator))]
public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponSO data;

    public int CurAmmo => Controller.CurAmmo;
    public int MaxAmmo => Stat.MaxAmmo;

    public WeaponSO Data => data;
    public WeaponStatHandler Stat { get; private set; }
    public WeaponController Controller { get; private set; }

    public Animator Anim { get; private set; }

    private void Awake()
    {
        if (data == null)
        {
            data = ResourceManager.Instance.Load<WeaponSO>(
                $"Data/SO/CharacterSO/{gameObject.name.Replace("(Clone)", "").Trim()}");
        }

        Stat = new WeaponStatHandler(data);
        Controller = GetComponent<WeaponController>();

        Anim = GetComponent<Animator>();

    }
}