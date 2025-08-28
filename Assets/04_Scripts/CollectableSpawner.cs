using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
    [Header("Collectable Prefabs to spawn")]
    public GameObject[] collectablePrefabs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("RandomCollectable");

        foreach (GameObject spawnPoint in spawnPoints)
        {
            if (collectablePrefabs.Length == 0) return;

            int index = Random.Range(0, collectablePrefabs.Length);

            Instantiate(collectablePrefabs[index], spawnPoint.transform);

        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
