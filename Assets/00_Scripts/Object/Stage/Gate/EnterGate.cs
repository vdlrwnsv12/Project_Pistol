using UnityEngine;

public class EnterGate : BaseGate
{
    [SerializeField] private GameObject door;
    
    public GameObject Door => door;

    private void Awake()
    {
        OnPassingGate += () => door.SetActive(true);
    }
}
