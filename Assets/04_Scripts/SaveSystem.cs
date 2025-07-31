using UnityEngine;
using System.IO;

public static class SaveSystem
{
    private static readonly string savePath = Application.persistentDataPath + "/inventory.json";

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
            InventoryData inventory = JsonUtility.FromJson<InventoryData>(json);
            return inventory;
        }
        else
        {
            Debug.Log("no save file found");
            return new InventoryData();
        }
    } 
}
