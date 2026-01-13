using UnityEngine;

public class EnemytClass : MonoBehaviour
{
    //Variables for all Enemies
    [SerializeField] protected float health = 100f;
    [SerializeField] protected float speed = 5f;
    [SerializeField] protected int damage = 10;
    [SerializeField] protected float attackRange = 1.5f;
    [SerializeField] protected float scrapDropAmount = 5f;
    [SerializeField] protected float slopDropAmount = 2f;
    [SerializeField] protected int enemySpawnAmount = 1;

    //Functions for all Enemies
    public virtual void EnemySpawn(Vector3 position)
    {
        // Logic for spawning enemy
        Instantiate(this, position, Quaternion.identity);
        Debug.Log("Enemy spawned at " + position);
    }
    public virtual void EnemyDeath()
    {
        // Logic for enemy death
        Debug.Log("Enemy has died.");
        Destroy(gameObject);
    }

    public virtual void EnemyTakeDamage(float amount)
    {
        health -= amount;
        Debug.Log("Enemy took " + amount + " damage.");

        if (health <= 0)
        {
            EnemyDeath();
        }
    }

    public virtual void EnemyAttack()
    {
        // Logic for enemy attack
        Debug.Log("Enemy attacks for " + damage + " damage.");
    }


}
