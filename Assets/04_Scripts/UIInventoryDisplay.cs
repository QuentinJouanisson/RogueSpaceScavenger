using UnityEngine;
using TMPro;


public class UIInventoryDisplay : MonoBehaviour
{
    [Header ("UI")]
    public TextMeshProUGUI itemCountText;

    [Header("ItemToDisplay")]
    public string itemId = "pileOfJunk";

    void Start()
    {
        UpdateDisplay();
    }

public void UpdateDisplay()
    {
        if (InventoryManager.Instance != null)
        {
            int count = InventoryManager.Instance.GetCount(itemId);
            itemCountText.text = $"{itemId}:{count}";
            
        }
        else
        {
            itemCountText.text = "InventaireIntrouvable";
        }
    }
    void Update()
    {
        
    }
}
