using UnityEngine;

public class CollectablesPickup : MonoBehaviour
{
    public string id = "pileOfJunk";
    public string displayName = "Pile of Junk";
    public int quantity = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InventoryManager.Instance.AddItem(id, displayName, quantity);
            Destroy(gameObject);
        }
    }    
}
