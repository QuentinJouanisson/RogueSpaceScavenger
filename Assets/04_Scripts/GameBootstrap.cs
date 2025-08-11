using UnityEngine;
using UnityEngine.SceneManagement;


public class GameBootstrap : MonoBehaviour
{
    private static bool hasLoadedGameScene = false;


    void Start()
    {
        if (!hasLoadedGameScene)
        {
            SceneManager.LoadScene("DemoLevelScene", LoadSceneMode.Additive);
            SceneManager.LoadScene("PersistantScene", LoadSceneMode.Additive);
            hasLoadedGameScene = true;
        }       
    }



    void Update()
    {
        
    }
}
