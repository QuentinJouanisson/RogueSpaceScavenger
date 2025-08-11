using UnityEngine;
using UnityEngine.SceneManagement;


public class GameBootstrap : MonoBehaviour
{

    void Start()
    {
        SceneManager.LoadScene("PersistentScene", LoadSceneMode.Additive);

        SceneManager.LoadScene("DemoLevelScene",LoadSceneMode.Additive);  
        
    }



    void Update()
    {
        
    }
}
