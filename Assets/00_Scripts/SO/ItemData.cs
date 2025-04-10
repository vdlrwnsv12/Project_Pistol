using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    public string id;
    [SerializeField] protected ApplyType type;
    public Sprite icon;
    public string itemName;
    public string description;
    public float cost;
}

public enum ApplyType
{
    Player,
    Weapon
}
