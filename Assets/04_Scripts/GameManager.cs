using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float GameOverDuration = 2f;
    
    //public CinemachineCamera virtualCamera;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TriggerGameOver()
    {
        FreezeCamera();
        Debug.Log("GameOver Triggered");
        Invoke(nameof(ReloadStartScene), GameOverDuration);       

        
        
    }
    private void FreezeCamera()
    {
        var virtualCamera = FindFirstObjectByType<CinemachineCamera>();
        if (virtualCamera != null)
        {
            virtualCamera.Follow = null;
        }
    }
    private void ReloadStartScene()
    {
        Debug.Log("reloading now");
        SceneManager.LoadScene(1);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
