using UnityEngine;
using System.IO;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    private string savePath => Application.persistentDataPath + "/inventory.json";

    public InventoryData inventory = new InventoryData();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadInventory();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AddItem(string id, string displayName, int quantity = 1)
    {
        var existing = inventory.items.Find(i => i.id == id);
        if (existing != null)
            existing.quantity = quantity;
        else
            inventory.items.Add(new Collectables(id, displayName, quantity));
        SaveInventory();
    }

    public void SaveInventory()
    {
        var json = JsonUtility.ToJson(inventory, true);
        File.WriteAllText(savePath, json);
    }

    public void LoadInventory()
    {
        if (File.Exists(savePath))
        {
            var json = File.ReadAllText(savePath);
            inventory = JsonUtility.FromJson<InventoryData>(json);
        }
    }
    public void ClearInventory()
    {
        inventory = new InventoryData();
        SaveInventory();
    }    
}
