public abstract class BaseItem
{
    protected ItemData itemData;

    protected BaseItem(ItemData itemData)
    {
        this.itemData = itemData;
    }

    public abstract void UseItem();
    
    public abstract void ApplyItemStat();
}
