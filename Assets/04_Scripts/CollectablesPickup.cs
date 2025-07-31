using UnityEngine;

public class CollectablesObject : MonoBehaviour
{
    public string id = "pileOfJunk";   
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InventoryData inventory = SaveSystem.LoadInventory();
            inventory.AddItem(id, 1);
            SaveSystem.SaveInventory(inventory);
            Destroy(gameObject);
        }
    }    
}
