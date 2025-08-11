using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InventoryManager.Instance.currentInventory = SaveSystem.LoadInventory();


            if(player != null)
        {
            var controller = player.GetComponent<MotoController>();
            if(controller != null)
            {
                controller.enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
