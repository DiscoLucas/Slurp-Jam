using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using System.Collections;

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

        numberOfEnemiesToSpawn = numberOfEnemiesToSpawn + (currentWave * 2);

        for (int i = 0; i < numberOfEnemiesToSpawn; i++)
        {
            enemySpawner.SpawnEnemy();
            yield return new WaitForSeconds(enemySpawner.spawnInterval);
        }

        enemiesRemaining = numberOfEnemiesToSpawn;
    }


    private void endWave()
    {
        isWaveActive = false;
        enemiesRemaining = 0;
        // Additional logic for ending the wave can be added here
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
}
