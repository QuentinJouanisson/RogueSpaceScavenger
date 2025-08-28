using Unity.VisualScripting;
using UnityEngine;

public class CollectablesObject : MonoBehaviour
{
    public AudioClip pickupSound;
    public float pickupSoundVolume;

    public string id = "Junk";
    public int quantity = 1;
    public bool randomValue = false;
    public int minQuantity = 1;
    public int maxQuantity = 10;
    

    private void OnTriggerEnter(Collider other)
    {
        int randomQuantity = Random.Range(minQuantity, maxQuantity);
        if (other.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, pickupSoundVolume);

            if(InventoryManager.Instance != null)
            {
                if(randomValue == false)
                {
                    InventoryManager.Instance.AddItem(id, quantity);
                }
                else
                {
                    InventoryManager.Instance.AddItem(id, randomQuantity);
                }
                
            }
            else
            {
                Debug.LogError("InventoryManager instance not found");
            }               
            
            Destroy(gameObject);
        }
    }    
}
