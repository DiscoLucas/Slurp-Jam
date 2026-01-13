using UnityEngine;

public class EnemytClass : MonoBehaviour
{
    //Variables for all Enemies
    [Header("Enemy Stats")]
    [SerializeField] protected string enemyName = "Enemy";
    [SerializeField] protected int enemyHealth = 100;
    [SerializeField] protected float enemySpeed = 5f;
    [SerializeField] protected int enemyDamage = 10;
    [SerializeField] protected float attackSpeed = 1f;
    protected float nextAttackTime = 0f;
    [SerializeField] protected float aggroRange = 10f;
    [SerializeField] protected float attackRange = 1.5f;
    [SerializeField] protected int scrapDropAmount = 5;
    [SerializeField] protected int slopDropAmount = 2;

    //Audio Variables
    [Header("Enemy Sounds")]
    [SerializeField] protected AudioClip deathSound;
    [SerializeField] protected AudioClip attackSound;
    protected AudioSource audioSource;


    //Functions for all Enemies
    public virtual void EnemyDeath()
    {
        // Logic for enemy death
        Debug.Log(enemyName + " has died.");
        Destroy(gameObject);
    }

    public virtual void EnemyTakeDamage(int damageToTake)
    {
        enemyHealth -= damageToTake;
        Debug.Log(enemyName + " took " + damageToTake + " damage.");

        if (enemyHealth <= 0)
        {
            EnemyDeath();
        }
    }

    public virtual void EnemyAttack()
    {
        // Logic for enemy attack
        // add logic for attack speed
        Debug.Log(enemyName + " attacks for " + enemyDamage + " damage.");
    }

    public virtual void EnemyDealDamage()
    {

    }


}
