using System;
using UnityEngine;

[Serializable]
public class EnemyType
{
    public GameObject enemyPrefab;
    public int firstEncounter = 0;
    [Range(0, 100)]
    public int spawnRate = 50;

}
