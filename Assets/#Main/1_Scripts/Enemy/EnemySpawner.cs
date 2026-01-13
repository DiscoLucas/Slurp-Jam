using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Reference to the enemy prefab to spawn
    public GameObject enemyPrefab;
    // Spawn interval in seconds
    public float spawnInterval = 2f;
    private float timer;
    //private void Update()
    //{
    //    timer += Time.deltaTime;
    //    if (timer >= spawnInterval)
    //    {
    //        SpawnEnemy();
    //        timer = 0f;
    //    }
    //}

    public void SpawnEnemy()
    {
        Instantiate(enemyPrefab, transform.position, transform.rotation);
    }

    public void ManualEnemySpawn()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SpawnEnemy();
            Debug.Log("Enemy Spawned Manually");
        }
    }

    public void Update()
    {
        ManualEnemySpawn();
    }
}
