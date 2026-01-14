using UnityEngine;
using UnityEngine.AI;

public class EnemytClass : MonoBehaviour
{
    //Variables for all Enemies
    [Header("Enemy Stats")]
    [SerializeField] protected string enemyName = "Enemy";
    [SerializeField] protected int enemyHealth = 100;
    [SerializeField] protected float enemySpeed = 5f;
    [SerializeField] protected int enemyDamage = 10;
    [SerializeField] protected float attackSpeed = 1f;
    [SerializeField] protected float attackRange = 1.5f;
    protected float nextAttackTime = 0f;
    [SerializeField] protected float aggroRange = 10f;
    [SerializeField] protected Transform enemyGoal;
    [SerializeField] protected Transform player;

    
    //NavMeshAgent for movement
    [SerializeField] protected NavMeshAgent navMeshAgent;


    //Drop Variables
    [SerializeField] protected int scrapDropAmount = 5;
    [SerializeField] protected int slopDropAmount = 2;
    [SerializeField] protected GameObject corpsePrefab;

    //Audio Variables
    [Header("Enemy Sounds")]
    [SerializeField] protected AudioClip deathSound;
    [SerializeField] protected AudioClip attackSound;
    protected AudioSource audioSource;

    private void Start()
    {
        //player = GameObject.FindWithTag("Player").transform;
        //enemyGoal = GameObject.FindGameObjectWithTag("EnemyGoal").transform;
        //Debug.Log("Enemy Goal found: " + enemyGoal.name);
        //navMeshAgent = GetComponent<NavMeshAgent>();
    }

    //Functions for all Enemies
    public virtual void EnemyDeath()
    {
        // Logic for enemy death
        Debug.Log(enemyName + " has died.");
        GameObject.Instantiate(corpsePrefab, transform.position, transform.rotation);
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

    //Movement Function towards EnemyGaol tag or Player if within aggro range
    protected Transform GetCurrentTarget()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= aggroRange)
            {
                return player;
            }
        }

        return enemyGoal;
    }

    public virtual void EnemyMoveTowardsTarget()
    {
        if (navMeshAgent == null)
            return;

        Transform target = GetCurrentTarget();
        if (target == null)
            return;

        navMeshAgent.speed = enemySpeed;
        navMeshAgent.SetDestination(target.position);
        Vector3 dir = (target.position - transform.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);

    }

    public virtual void CreateAgent(Transform player, Transform goal)
    {
        this.player = player;
        this.enemyGoal = goal;
    }





}
