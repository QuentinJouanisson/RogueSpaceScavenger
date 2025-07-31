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

    //public static T Find<T>(this IList<T> list) 
    //{
    //    return list[0] ;
    //}
    public void SaveInventory()
    {
        SaveSystem.SaveInventory(currentInventory);        
    }

    public void LoadInventory()
    {
        currentInventory  = SaveSystem.LoadInventory();
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
