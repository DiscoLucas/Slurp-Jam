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

    [SerializeField] protected Transform appearesObject;
    private Rigidbody rb;

    //NavMeshAgent for movement
    [SerializeField] protected NavMeshAgent navMeshAgent;
    [SerializeField] protected float knockbackForce = 5f;


    //Drop Variables
    [Range(1,100)]
    [SerializeField] protected int dropchance = 50;
    [SerializeField] protected int scrapMaxDropAmount = 5;
    [SerializeField] protected int slopMaxDropAmount = 2;
    [SerializeField] protected GameObject corpsePrefab;

    //Audio Variables
    [Header("Enemy Sounds")]
    [SerializeField] protected AudioClip deathSound;
    [SerializeField] protected AudioClip attackSound;
    protected AudioSource audioSource;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        //player = GameObject.FindWithTag("Player").transform;
        //enemyGoal = GameObject.FindGameObjectWithTag("EnemyGoal").transform;
        //Debug.Log("Enemy Goal found: " + enemyGoal.name);
        //navMeshAgent = GetComponent<NavMeshAgent>();
        enemyGoal = GameObject.FindGameObjectWithTag("EnemyGoal")?.transform;
    }

    private void Update()
    {
        EnemyMoveTowardsTarget();
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            EnemyAttack();
        }
    }

    //Functions for all Enemies
    public virtual void EnemyDeath()
    {
        // Logic for enemy death
        Debug.Log(enemyName + " has died.");
        // Spawn corpse with Y offset to account for NavMesh base offset
        Vector3 corpsePosition = transform.position;
        if (navMeshAgent != null)
        {
            corpsePosition.y += navMeshAgent.baseOffset;
        }
        GameObject corpse = GameObject.Instantiate(corpsePrefab, corpsePosition, transform.rotation);
        Debug.Log("Corpse instantiated at " + corpsePosition + ", corpse actual position: " + corpse.transform.position + ", enemy position: " + transform.position);
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
        else
        {
            Vector3 knockbackDirection = (transform.position - player.position).normalized;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
        }
    }

    public virtual void EnemyAttack()
    {
        
    }


    protected virtual void EnemyDealDamage(Collider other)
    {
        if (other.CompareTag("EnemyGoal"))
        {
            Debug.Log("Arrow hit for " + enemyDamage + " damage to " + GameObject.FindGameObjectWithTag("EnemyGoal"));
        }
        else if (other.CompareTag("Player"))
        {
            Debug.Log("Arrow hit for " + enemyDamage + " damage to " + GameObject.FindGameObjectWithTag("Player"));
            PlayerContainer player = other.GetComponent<PlayerContainer>();
            if (player != null)
            {
                player.TakeDamage(enemyDamage);
            }
        }
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

        //Roate appearesObject to face movement direction
        if (appearesObject != null)
        {
            Vector3 moveDirection = navMeshAgent.velocity;
            if (moveDirection.sqrMagnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                appearesObject.rotation = Quaternion.Slerp(appearesObject.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }

    }

    public virtual void CreateAgent(Transform player, Transform goal)
    {
        this.player = player;
        this.enemyGoal = goal;
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyDealDamage(other);
    }





}
