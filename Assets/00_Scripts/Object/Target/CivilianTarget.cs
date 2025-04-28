using UnityEngine;

public class CivilianTarget : BaseTarget
{
    [Header("시민일 경우 메테리얼")]
    [SerializeField] private Material civilianMaterial;

    protected override void Start()
    {
        base.Start();

        if (data.Name == "Civilian")
        {
            Renderer rend = GetComponentInChildren<Renderer>();
            if (rend != null)
            {
                rend.material = civilianMaterial;
            }
        }
    }
}
