using DataDeclaration;
using UnityEngine;

public class PoolableResource : MonoBehaviour, IPoolable
{
    [field: SerializeField] public bool IsAutoReturn { get; private set; } = true;
    [field: SerializeField] public float ReturnTime { get; private set; } = 5f;

    public GameObject GameObject => gameObject;
    public int ResourceInstanceID => gameObject.GetInstanceID();
}
