using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Reference to the enemy prefabs to spawn
    public GameObject enemyPrefab;
    // Spawn interval in seconds
    public float spawnInterval = 1f;
    private float timer;
    public Transform enemyGoal;
    public Transform player;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        enemyGoal = GameObject.FindGameObjectWithTag("EnemyGoal").transform;
        Debug.Log("Enemy Goal found: " + enemyGoal.name);
    }

    private void Start()
    {
        Grunt gruntComponent = enemyPrefab.GetComponent<Grunt>();
    }

    public void SpawnEnemy(GameObject enemyPrefab)
    {
        GameObject obj = Instantiate(enemyPrefab, transform.position, transform.rotation);
        obj.GetComponent<EnemytClass>().CreateAgent(player, enemyGoal);
        //selec a random material if the enemy is a grunt
        Grunt gruntComponent = enemyPrefab.GetComponent<Grunt>();
        if (gruntComponent != null)
        {
            gruntComponent.SelectRandomMaterialForGunt();
        }

    }

    public void ManualEnemySpawn()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SpawnEnemy(enemyPrefab);
            Debug.Log("Enemy Spawned Manually");
        }
    }
}
