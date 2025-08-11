using UnityEngine;
using System.IO;

public static class SaveSystem
{
    private static readonly string savePath = Application.persistentDataPath + "/inventory.json";
    private static readonly string backupPath = Application.persistentDataPath + "/inventory_backup.json";

    public static void SaveInventory(InventoryData inventory)
    {
            string json = JsonUtility.ToJson(inventory, true);
            File.WriteAllText(savePath, json); 
            Debug.Log("inventory Saved");               
    }

    public static InventoryData LoadInventory()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            InventoryData loaded = JsonUtility.FromJson<InventoryData>(json);
            if (loaded == null)
            {
                Debug.LogWarning("Save file found but could not be parsed, loading empty inventory");
                return new InventoryData();
            }
            return loaded;
        }
        else
        {
            Debug.Log("No save file found, loading empty inventory");
            return new InventoryData();
        }
    }

    public static void ClearSave()
    {
        if(File.Exists(savePath)) File.Delete(savePath);
        if (File.Exists(backupPath)) File.Delete(backupPath);
        Debug.Log("Inventory save cleared");
    }
}
