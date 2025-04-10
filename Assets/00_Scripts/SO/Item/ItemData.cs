using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    [field: SerializeField] public string Id { get; private set; }
    [Tooltip("아이템 적용 타겟"), SerializeField] protected ApplyType type;
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public string ItemName { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: Tooltip("아이템 구매 비용"), SerializeField] public float Cost { get; private set; }
}

public enum ApplyType
{
    Player,
    Weapon
}