[System.Serializable]
public class CollectablesData
{
    public string id;
    public int quantity;

    public CollectablesData(string id, int quantity = 1)
    {
        this.id = id;        
        this.quantity = quantity;
    }
}
