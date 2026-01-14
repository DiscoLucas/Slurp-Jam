using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using System.Collections;
using System.Linq;

public class WaveManager : MonoBehaviour
{
    public int currentWave;
    public bool isWaveActive;
    public bool isPreparingForNextWave;
    public bool isBossWave;
    public int enemiesRemaining;
    public int numberOfEnemiesToSpawn = 5;
    //public float spawnInterval = 1.0f;
    [SerializeField] private EnemySpawner enemySpawner;
    [Header("Array of Enemy Types")]
    [SerializeField] private EnemyType[] enemyTypes;
    [SerializeField] private int bossWaveInterval = 5;
    [SerializeField] private int playerHealthRegenPerWave = 10;


    private void Awake()
    {
        currentWave = 0;
        isWaveActive = false;
        isPreparingForNextWave = false;
        isBossWave = false;
        enemiesRemaining = 0;
    }

    private void Start()
    {
        prepareForWave();
    }

    private void prepareForWave()
    {
        isPreparingForNextWave = true;
        // Additional logic for preparing the wave can be added here
    }

    private void startWave()
    {
        StartCoroutine(StartWaveCorotine());
    }
    private IEnumerator StartWaveCorotine()
    {
        isWaveActive = true;
        isPreparingForNextWave = false;
        currentWave++;

        numberOfEnemiesToSpawn += currentWave * 2;

        for (int i = 0; i < numberOfEnemiesToSpawn; i++)
        {
            EnemyType type = GetRandomEnemyType();
            if (type != null)
            {
                enemySpawner.SpawnEnemy(type.enemyPrefab);
            }

            yield return new WaitForSeconds(enemySpawner.spawnInterval);
        }

        enemiesRemaining = numberOfEnemiesToSpawn;
    }



    private void endWave()
    {
        isWaveActive = false;
        enemiesRemaining = 0;
        // Additional logic for ending the wave can be added here
        PlayerContainer playerContainer = GetComponent<PlayerContainer>();
        playerContainer.Heal(playerHealthRegenPerWave);
    }

    //manually start wave for testing
    [ContextMenu("StarWave")]
    public void ManualStartWave()
    {
        startWave();
        Debug.Log("Wave " + currentWave + " Started Manually");
    }

    //kill all enemies for testing
    [ContextMenu("EndWave")]
    public void ManualEndWave()
    {
        endWave();
        Debug.Log("Wave " + currentWave + " Ended Manually");
    }

    private EnemyType GetRandomEnemyType()
    {
        // Only enemies unlocked for this wave
        var availableEnemies = enemyTypes
            .Where(e => currentWave >= e.firstEncounter)
            .ToList();

        if (availableEnemies.Count == 0)
            return null;

        int totalWeight = availableEnemies.Sum(e => e.spawnRate);
        int randomValue = Random.Range(0, totalWeight);

        int runningTotal = 0;
        foreach (var enemy in availableEnemies)
        {
            runningTotal += enemy.spawnRate;
            if (randomValue < runningTotal)
                return enemy;
        }

        return availableEnemies[0];
    }

    //Manual damage to enemies
    // Manual damage to enemies
    [ContextMenu("DamageAllEnemies")]
    public void DamageAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemyObj in enemies)
        {
            EnemytClass enemy = enemyObj.GetComponent<EnemytClass>();
            if (enemy != null)
            {
                enemy.EnemyTakeDamage(9999);
            }
        }

        Debug.Log("All Enemies Damaged");
    }

}
