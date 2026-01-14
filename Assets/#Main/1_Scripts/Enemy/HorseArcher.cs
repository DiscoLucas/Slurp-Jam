using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.AI;

public class HorseArcher : EnemytClass
{
    //public GameObject enemyGoal; // Reference to the enemy goal object
    public GameObject arrowPrefab;
    public float arrowSpeed = 15f;
    [SerializeField] private float arrowLifetime = 2.5f;

    //HorseArcher spicifics
    [SerializeField] private float orbitDistance = 2f;
    [SerializeField] private float orbitSpeed = 2f;
    [SerializeField] private float orbitSmoothing = 2f;
    [SerializeField] private float thresholdDistance = 0.5f;

    private float orbitAngle;
    private float orbitDirection;

    private float navMeshMaxDist = 2f;  
    private float ratio = 8f;

    private void Start()
    {
        orbitDirection = Random.value > 0.5f ? 1f : -1f;
        orbitSpeed = enemySpeed * 0.95f;
    }

    protected Transform GetAttackTarget()
    {
        if (player != null)
        {
            float distToPlayer = Vector3.Distance(transform.position, player.position);
            if (distToPlayer <= attackRange)
                return player;
        }

        return enemyGoal;
    }


    public override void EnemyAttack()
    {
        Transform target = GetAttackTarget();
        if (target == null)
            return;

        if (Time.time < nextAttackTime)
            return;

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > attackRange)
            return;

        nextAttackTime = Time.time + attackSpeed;

        Vector3 direction = (target.position - transform.position).normalized;

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

    private enum MoveState { Approach, Orbit }
    private MoveState currentMoveState = MoveState.Approach;


    public override void EnemyMoveTowardsTarget()
    {
        if (navMeshAgent == null)
            return;

        Transform target = GetCurrentTarget();
        if (target == null)
            return;

        float distance = Vector3.Distance(transform.position, target.position);

        float approachThreshold = attackRange + thresholdDistance;
        float orbitThreshold = attackRange - thresholdDistance;

        // 🔁 Update state
        if (currentMoveState == MoveState.Approach && distance < orbitThreshold)
            currentMoveState = MoveState.Orbit;
        else if (currentMoveState == MoveState.Orbit && distance > approachThreshold)
            currentMoveState = MoveState.Approach;

        navMeshAgent.speed = enemySpeed;
        navMeshAgent.isStopped = false;

        if (currentMoveState == MoveState.Approach)
        {
            navMeshAgent.SetDestination(target.position);
        }
        else // Orbit
        {
            orbitAngle += orbitSpeed * orbitDirection * Time.deltaTime;

            Vector3 offset = new Vector3(
                Mathf.Cos(orbitAngle),
                0,
                Mathf.Sin(orbitAngle)
            ) * orbitDistance;

            Vector3 orbitPos = target.position + offset;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(orbitPos, out hit, navMeshMaxDist, NavMesh.AllAreas))
            {
                navMeshAgent.SetDestination(hit.position);
            }
        }

        // Always face target
        Vector3 dir = (target.position - transform.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * ratio);
    }






}
