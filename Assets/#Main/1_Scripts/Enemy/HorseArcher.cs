using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class HorseArcher : EnemytClass
{
    //public GameObject enemyGoal; // Reference to the enemy goal object
    public GameObject arrowPrefab;
    public float arrowSpeed = 15f;
    [SerializeField] private float arrowLifetime = 2.5f;


    private void Start()
    {
        //enemyGoal = GameObject.FindWithTag("EnemyGoal");
    }

    private void Update()
    {
        EnemyMoveTowardsTarget();
        if (Vector3.Distance(transform.position, enemyGoal.transform.position) <= attackRange)
        {
            EnemyAttack();
        }
    }

    private void MoveTowardsGoal()
    {
        if (enemyGoal != null)
        {
            // Move the grunt towards the enemy goal
            transform.position = Vector3.MoveTowards(transform.position, enemyGoal.transform.position, enemySpeed * Time.deltaTime);
        }
    }

    public override void EnemyAttack()
    {
        if (enemyGoal == null)
            return;

        if (Time.time < nextAttackTime)
            return;

        if (Vector3.Distance(transform.position, enemyGoal.transform.position) > attackRange)
            return;

        nextAttackTime = Time.time + attackSpeed;

        Vector3 direction = (enemyGoal.transform.position - transform.position).normalized;

        GameObject arrow = Instantiate(
            arrowPrefab,
            transform.position,
            Quaternion.LookRotation(direction)
        );

        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.linearVelocity = direction * arrowSpeed;

        Arrow arrowScript = arrow.GetComponent<Arrow>();
        arrowScript.SetDamage(enemyDamage);

        Destroy(arrow, arrowLifetime);
    }



}
