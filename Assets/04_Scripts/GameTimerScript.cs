using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTimerScript : MonoBehaviour
{
    [Header("Timer Settings")]
    public float gameDuration = 60f;
    private float currentTime;

    [Header("UI")]
    public TextMeshProUGUI timerText;

    void Start()
    {
        SaveSystem.ClearSave();
        currentTime = gameDuration;
        UpdateTimerUI();
        
    }


    // Update is called once per frame
    void Update()
    {
        if (currentTime > 0f)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerUI();

            if (currentTime <= 0f)
            {
                currentTime = 0f;
                EndGame();
            }
        }
        
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    void EndGame()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.SaveInventory();

        }
        else
        {
            Debug.LogWarning("inventoryManagerIntrouvable");
        }

        SceneManager.LoadScene("WorkshopScene");
    }

}
