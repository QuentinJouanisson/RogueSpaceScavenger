using System.Collections.Generic;

[System.Serializable]
public class InventoryData
{
    public List<CollectablesData> collectedItems = new List<CollectablesData>();
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
        collectedItems.Add(new CollectablesData(id, amount));
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
