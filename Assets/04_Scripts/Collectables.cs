[System.Serializable]
public class CollectableItem
{
    public string id;
    public int quantity;
    public CollectableItem() { }

    public CollectableItem(string id, int quantity = 1)
    {
        this.id = id;        
        this.quantity = quantity;
    }
}
