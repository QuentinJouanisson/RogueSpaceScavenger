using System.Collections.Generic;

[System.Serializable]
public class InventoryData
{
    public List<CollectableItem> collectedItems = new List<CollectableItem>();
    public void AddItem(string id, int amount = 1)
    {
        foreach(var item in collectedItems)
        {
            if(item.id == id)
            {
                item.quantity += amount;
                return;
            }
        }
        collectedItems.Add(new CollectableItem(id, amount));
    }

    public int GetCount(string id)
    {
        foreach(var item in collectedItems)
        {
            if(item.id == id)
                return item.quantity;
        }
        return 0;
    }    
}
