using UnityEngine;

public class CollectablesObject : MonoBehaviour
{
    public AudioClip pickupSound;
    public float pickupSoundVolume;

    public string id = "Junk";   
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, pickupSoundVolume);

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
