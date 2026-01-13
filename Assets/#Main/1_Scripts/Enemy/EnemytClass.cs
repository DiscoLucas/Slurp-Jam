using UnityEngine;

public class EnemytClass : MonoBehaviour
{
    //Variables for all Enemies
    [SerializeField] protected string enemyName = "Enemy";
    [SerializeField] protected float health = 100f;
    [SerializeField] protected float speed = 5f;
    [SerializeField] protected int damage = 10;
    [SerializeField] protected float aggroRange = 10f;
    [SerializeField] protected float attackRange = 1.5f;
    [SerializeField] protected float scrapDropAmount = 5f;
    [SerializeField] protected float slopDropAmount = 2f;

    //Audio Variables
    //[SerializeField] protected AudioClip deathSound;
    //[SerializeField] protected AudioClip attackSound;
    //protected AudioSource audioSource;


    //Functions for all Enemies
    public virtual void EnemyDeath()
    {
        // Logic for enemy death
        Debug.Log(enemyName + " has died.");
        Destroy(gameObject);
    }

    public virtual void EnemyTakeDamage(float amount)
    {
        health -= amount;
        Debug.Log(enemyName + " took " + amount + " damage.");

        if (health <= 0)
        {
            EnemyDeath();
        }
    }

    public virtual void EnemyAttack()
    {
        // Logic for enemy attack
        Debug.Log(enemyName + " attacks for " + damage + " damage.");
    }


}
