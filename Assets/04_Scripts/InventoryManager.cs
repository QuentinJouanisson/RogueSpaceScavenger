using UnityEngine;
using System.IO;
using NUnit.Framework;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public InventoryData currentInventory;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadInventory();
            if(currentInventory == null)
                currentInventory = new InventoryData();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AddItem(string id, int amount = 1)
    {
       currentInventory.AddItem(id, amount);
       SaveInventory();        
    }     
    public void SaveInventory()
    {
        SaveSystem.SaveInventory(currentInventory);        
    }

    public void LoadInventory()
    {
        currentInventory  = SaveSystem.LoadInventory() ?? new InventoryData();
    }
    public void ClearInventory()
    {
        currentInventory = new InventoryData();
        SaveInventory();
    }    
    public int GetCount(string id)
    {
        return currentInventory.GetCount(id);
    }    
}
