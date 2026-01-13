using UnityEngine;
using UnityEngine.Events;

public class WaveManager : MonoBehaviour
{
    public int currentWave;
    public bool isWaveActive;
    public bool isPreparingForNextWave;
    public bool isBossWave;
    public int enemiesRemaining;
    public int numberOfEnemiesToSpawn;
    [SerializeField] private EnemySpawner enemySpawner;

    private void Start()
    {
        currentWave = 0;
        isWaveActive = false;
        isPreparingForNextWave = false;
        isBossWave = false;
        enemiesRemaining = 0;
        numberOfEnemiesToSpawn = 0;
    }

    private void prepareForWave()
    {
        isPreparingForNextWave = true;
        // Additional logic for preparing the wave can be added here
    }

    private void startWave()
    {
        isWaveActive = true;
        isPreparingForNextWave = false;
        currentWave++;
        // Additional logic for starting the wave can be added here

        //activate the enemy spawner
        // spawn enemy
        if (enemySpawner != null)
        {
            for (int i = 0; i < numberOfEnemiesToSpawn; i++)
            {
                enemySpawner.SpawnEnemy();
            }
            enemiesRemaining = numberOfEnemiesToSpawn;
        }
    }

    private void endWave()
    {
        isWaveActive = false;
        enemiesRemaining = 0;
        // Additional logic for ending the wave can be added here
    }

    //manually start wave for testing
    public void ManualStartWave()
    {
        if (Input.GetKeyDown("space") && !isWaveActive)
        {
            startWave();
            Debug.Log("Wave " + currentWave + " Started Manually");
        }
    }

    public void Update()
    {
        ManualStartWave();
    }

}
