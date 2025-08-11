using UnityEngine;

public class CollectablesObject : MonoBehaviour
{
    public string id = "Junk";   
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(InventoryManager.Instance != null)
            {
                InventoryManager.Instance.AddItem(id, 1);
            }
            else
            {
                Debug.LogError("InventoryManager instance not found");
            }               
            
            Destroy(gameObject);
        }
    }    
}
