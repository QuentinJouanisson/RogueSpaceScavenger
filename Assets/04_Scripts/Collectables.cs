[System.Serializable]
public class Collectables
{
    public string id;
    public string displayName;
    public int quantity;

    public Collectables(string id, string displayName, int quantity = 1)
    {
        this.id = id;
        this.displayName = displayName;
        this.quantity = quantity;
    }
}
